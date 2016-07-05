.. _data-protection-implementation-context-headers:

Context headers
===============

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

Background and theory
---------------------

背景与理论
---------------------

In the data protection system, a "key" means an object that can provide authenticated encryption services. Each key is identified by a unique id (a GUID), and it carries with it algorithmic information and entropic material. It is intended that each key carry unique entropy, but the system cannot enforce that, and we also need to account for developers who might change the key ring manually by modifying the algorithmic information of an existing key in the key ring. To achieve our security requirements given these cases the data protection system has a concept of `cryptographic agility <http://research.microsoft.com/apps/pubs/default.aspx?id=121045>`_, which allows securely using a single entropic value across multiple cryptographic algorithms.

Most systems which support cryptographic agility do so by including some identifying information about the algorithm inside the payload. The algorithm's OID is generally a good candidate for this. However, one problem that we ran into is that there are multiple ways to specify the same algorithm: "AES" (CNG) and the managed Aes, AesManaged, AesCryptoServiceProvider, AesCng, and RijndaelManaged (given specific parameters) classes are all actually the same thing, and we'd need to maintain a mapping of all of these to the correct OID. If a developer wanted to provide a custom algorithm (or even another implementation of AES!), he'd have to tell us its OID. This extra registration step makes system configuration particularly painful.

Stepping back, we decided that we were approaching the problem from the wrong direction. An OID tells you what the algorithm is, but we don't actually care about this. If we need to use a single entropic value securely in two different algorithms, it's not necessary for us to know what the algorithms actually are. What we actually care about is how they behave. Any decent symmetric block cipher algorithm is also a strong pseudorandom permutation (PRP): fix the inputs (key, chaining mode, IV, plaintext) and the ciphertext output will with overwhelming probability be distinct from any other symmetric block cipher algorithm given the same inputs. Similarly, any decent keyed hash function is also a strong pseudorandom function (PRF), and given a fixed input set its output will overwhelmingly be distinct from any other keyed hash function.

We use this concept of strong PRPs and PRFs to build up a context header. This context header essentially acts as a stable thumbprint over the algorithms in use for any given operation, and it provides the cryptographic agility needed by the data protection system. This header is reproducible and is used later as part of the :ref:`subkey derivation process <data-protection-implementation-subkey-derivation>`. There are two different ways to build the context header depending on the modes of operation of the underlying algorithms.


CBC-mode encryption + HMAC authentication
-----------------------------------------

CBC 模式加密 + HMAC 认证
-----------------------------------------

.. _data-protection-implementation-context-headers-cbc-components:

The context header consists of the following components:

* [16 bits] The value 00 00, which is a marker meaning "CBC encryption + HMAC authentication".
* [32 bits] The key length (in bytes, big-endian) of the symmetric block cipher algorithm.
* [32 bits] The block size (in bytes, big-endian) of the symmetric block cipher algorithm.
* [32 bits] The key length (in bytes, big-endian) of the HMAC algorithm. (Currently the key size always matches the digest size.)
* [32 bits] The digest size (in bytes, big-endian) of the HMAC algorithm.
* EncCBC(K\ :sub:`E`, IV, ""), which is the output of the symmetric block cipher algorithm given an empty string input and where IV is an all-zero vector. The construction of K\ :sub:`E` is described below.
* MAC(K\ :sub:`H`, ""), which is the output of the HMAC algorithm given an empty string input. The construction of K\ :sub:`H` is described below.

Ideally we could pass all-zero vectors for K\ :sub:`E` and K\ :sub:`H`. However, we want to avoid the situation where the underlying algorithm checks for the existence of weak keys before performing any operations (notably DES and 3DES), which precludes using a simple or repeatable pattern like an all-zero vector.

Instead, we use the NIST SP800-108 KDF in Counter Mode (see `NIST SP800-108 <http://csrc.nist.gov/publications/nistpubs/800-108/sp800-108.pdf>`_, Sec. 5.1) with a zero-length key, label, and context and HMACSHA512 as the underlying PRF. We derive | K\ :sub:`E` | + | K\ :sub:`H` | bytes of output, then decompose the result into K\ :sub:`E` and K\ :sub:`H` themselves. Mathematically, this is represented as follows.

( K\ :sub:`E` || K\ :sub:`H` ) = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = "")

Example: AES-192-CBC + HMACSHA256
"""""""""""""""""""""""""""""""""

As an example, consider the case where the symmetric block cipher algorithm is AES-192-CBC and the validation algorithm is HMACSHA256. The system would generate the context header using the following steps.

First, let ( K\ :sub:`E` || K\ :sub:`H` ) = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = ""), where | K\ :sub:`E` | = 192 bits and | K\ :sub:`H` | = 256 bits per the specified algorithms. This leads to K\ :sub:`E` = 5BB6..21DD and K\ :sub:`H` = A04A..00A9 in the example below::
  
  5B B6 C9 83 13 78 22 1D 8E 10 73 CA CF 65 8E B0
  61 62 42 71 CB 83 21 DD A0 4A 05 00 5B AB C0 A2
  49 6F A5 61 E3 E2 49 87 AA 63 55 CD 74 0A DA C4
  B7 92 3D BF 59 90 00 A9

Next, compute Enc\ :sub:`CBC` (K\ :sub:`E`, IV, "") for AES-192-CBC given IV = 0* and K\ :sub:`E` as above.

result := F474B1872B3B53E4721DE19C0841DB6F

Next, compute MAC(K\ :sub:`H`, "") for HMACSHA256 given K\ :sub:`H` as above.

result := D4791184B996092EE1202F36E8608FA8FBD98ABDFF5402F264B1D7211536220C

This produces the full context header below::

  00 00 00 00 00 18 00 00 00 10 00 00 00 20 00 00
  00 20 F4 74 B1 87 2B 3B 53 E4 72 1D E1 9C 08 41
  DB 6F D4 79 11 84 B9 96 09 2E E1 20 2F 36 E8 60
  8F A8 FB D9 8A BD FF 54 02 F2 64 B1 D7 21 15 36
  22 0C

This context header is the thumbprint of the authenticated encryption algorithm pair (AES-192-CBC encryption + HMACSHA256 validation). The components, as described :ref:`above <data-protection-implementation-context-headers-cbc-components>` are: 

* the marker (00 00)
* the block cipher key length (00 00 00 18)
* the block cipher block size (00 00 00 10)
* the HMAC key length (00 00 00 20)
* the HMAC digest size (00 00 00 20)
* the block cipher PRP output (F4 74 - DB 6F) and 
* the HMAC PRF output (D4 79 - end).

.. NOTE::
   The CBC-mode encryption + HMAC authentication context header is built the same way regardless of whether the algorithms implementations are provided by Windows CNG or by managed SymmetricAlgorithm and KeyedHashAlgorithm types. This allows applications running on different operating systems to reliably produce the same context header even though the implementations of the algorithms differ between OSes. (In practice, the KeyedHashAlgorithm doesn't have to be a proper HMAC. It can be any keyed hash algorithm type.)

Example: 3DES-192-CBC + HMACSHA1
""""""""""""""""""""""""""""""""

First, let ( K\ :sub:`E` || K\ :sub:`H` ) = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = ""), where | K\ :sub:`E` | = 192 bits and | K\ :sub:`H` | = 160 bits per the specified algorithms. This leads to K\ :sub:`E` = A219..E2BB and K\ :sub:`H` = DC4A..B464 in the example below::

  A2 19 60 2F 83 A9 13 EA B0 61 3A 39 B8 A6 7E 22
  61 D9 F8 6C 10 51 E2 BB DC 4A 00 D7 03 A2 48 3E
  D1 F7 5A 34 EB 28 3E D7 D4 67 B4 64

Next, compute Enc\ :sub:`CBC` (K\ :sub:`E`, IV, "") for 3DES-192-CBC given IV = 0* and K\ :sub:`E` as above.

result := ABB100F81E53E10E

Next, compute MAC(K\ :sub:`H`, "") for HMACSHA1 given K\ :sub:`H` as above.

result := 76EB189B35CF03461DDF877CD9F4B1B4D63A7555

This produces the full context header which is a thumbprint of the authenticated encryption algorithm pair (3DES-192-CBC encryption + HMACSHA1 validation), shown below::

  00 00 00 00 00 18 00 00 00 08 00 00 00 14 00 00
  00 14 AB B1 00 F8 1E 53 E1 0E 76 EB 18 9B 35 CF
  03 46 1D DF 87 7C D9 F4 B1 B4 D6 3A 75 55

The components break down as follows:

* the marker (00 00)
* the block cipher key length (00 00 00 18)
* the block cipher block size (00 00 00 08)
* the HMAC key length (00 00 00 14)
* the HMAC digest size (00 00 00 14)
* the block cipher PRP output (AB B1 - E1 0E) and 
* the HMAC PRF output (76 EB - end).

Galois/Counter Mode encryption + authentication
-----------------------------------------------

Galois/Counter 模式加密 + 认证
-----------------------------------------------

The context header consists of the following components:

上下文 header 由以下组成：

* [16 bits] The value 00 01, which is a marker meaning "GCM encryption + authentication".
* [32 bits] The key length (in bytes, big-endian) of the symmetric block cipher algorithm.
* [32 bits] The nonce size (in bytes, big-endian) used during authenticated encryption operations. (For our system, this is fixed at nonce size = 96 bits.)
* [32 bits] The block size (in bytes, big-endian) of the symmetric block cipher algorithm. (For GCM, this is fixed at block size = 128 bits.)
* [32 bits] The authentication tag size (in bytes, big-endian) produced by the authenticated encryption function. (For our system, this is fixed at tag size = 128 bits.)
* [128 bits] The tag of Enc\ :sub:`GCM` (K\ :sub:`E`, nonce, ""), which is the output of the symmetric block cipher algorithm given an empty string input and where nonce is a 96-bit all-zero vector. 

K\ :sub:`E` is derived using the same mechanism as in the CBC encryption + HMAC authentication scenario. However, since there is no K\ :sub:`H` in play here, we essentially have | K\ :sub:`H` | = 0, and the algorithm collapses to the below form.

K\ :sub:`E` = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = "")

Example: AES-256-GCM
""""""""""""""""""""

First, let K\ :sub:`E` = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = ""), where | K\ :sub:`E` | = 256 bits.

首先，假设 K\ :sub:`E` = SP800_108_CTR(prf = HMACSHA512, key = "", label = "", context = "") ，其中 | K\ :sub:`E` | = 256 bits 。

K\ :sub:`E` := 22BC6F1B171C08C4AE2F27444AF8FC8B3087A90006CAEA91FDCFB47C1B8733B8

Next, compute the authentication tag of Enc\ :sub:`GCM` (K\ :sub:`E`, nonce, "") for AES-256-GCM given nonce = 096 and K\ :sub:`E` as above.

接着为 AES-256-GCM 计算 Enc\ :sub:`GCM` (K\ :sub:`E`, nonce, "") 的身份验证标签，其中 nonce = 096， K\ :sub:`E`，如上所述。

result := E7DCCE66DF855A323A6BB7BD7A59BE45

This produces the full context header below::

  00 01 00 00 00 20 00 00 00 0C 00 00 00 10 00 00
  00 10 E7 DC CE 66 DF 85 5A 32 3A 6B B7 BD 7A 59
  BE 45

这将产生完整的 context header，如下::

  00 01 00 00 00 20 00 00 00 0C 00 00 00 10 00 00
  00 10 E7 DC CE 66 DF 85 5A 32 3A 6B B7 BD 7A 59
  BE 45
  
The components break down as follows:

 * the marker (00 01)
 * the block cipher key length (00 00 00 20)
 * the nonce size (00 00 00 0C)
 * the block cipher block size (00 00 00 10)
 * the authentication tag size (00 00 00 10) and 
 * the authentication tag from running the block cipher (E7 DC - end).

分解说明：

 * 特征标记 (00 01)；
 * 块密钥长度 (00 00 00 20)；
 * 随机数尺寸 (00 00 00 0C)；
 * 块密码块尺寸 (00 00 00 10)；
 * 身份验证标签尺寸 (00 00 00 10)；
 * 来自运行的块密钥的身份验证标签 (E7 DC - end)。