.. _data-protection-implementation-authenticated-encryption-details:

Authenticated encryption details.
=================================

身份验证加密详解
=================================

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

Calls to IDataProtector.Protect are authenticated encryption operations. The Protect method offers both confidentiality and authenticity, and it is tied to the purpose chain that was used to derive this particular IDataProtector instance from its root IDataProtectionProvider.

调用 IDataProtector.Protect 进行身份验证加密操作。Protect 方法为我们带来了机密性和真实性，它通过关联作用链从它自己的根 IDataProtectionProvider 中获取特定的 IDataProtector 实例。

IDataProtector.Protect takes a byte[] plaintext parameter and produces a byte[] protected payload, whose format is described below. (There is also an extension method overload which takes a string plaintext parameter and returns a string protected payload. If this API is used the protected payload format will still have the below structure, but it will be `base64url-encoded <https://tools.ietf.org/html/rfc4648#section-5>`_.)

IDataProtector.Protect 获取一个 byte[] 的铭文参数并返回一个 byte[] 的受保护载荷，其格式如下所述（还有个重载的扩展方法可以用，获取一个明文字符串参数并返回一个受保护的载荷。如果将此 API 用于受保护载荷的格式，则将依旧具有以下结构，但那将是 `base64url-encoded <https://tools.ietf.org/html/rfc4648#section-5>` 的）。

Protected payload format
------------------------

受保护的负载格式
------------------------

The protected payload format consists of three primary components:

* A 32-bit magic header that identifies the version of the data protection system.
* A 128-bit key id that identifies the key used to protect this particular payload.
* The remainder of the protected payload is :ref:`specific to the encryptor encapsulated by this key <data-protection-implementation-subkey-derivation>`. In the example below the key represents an AES-256-CBC + HMACSHA256 encryptor, and the payload is further subdivided as follows:
  * A 128-bit key modifier.
  * A 128-bit initialization vector.
  * 48 bytes of AES-256-CBC output.
  * An HMACSHA256 authentication tag.

受保护的载荷格式有三个主要部分组成：

* 32 位的 magic header 用于标识数据保护系统的版本。
* 128 位的密钥 ID 用于标识这个受保护的特性载荷。
* 受保护载荷的余下部分是 :ref:`特定的对该密钥进行封装的加密机 <data-protection-implementation-subkey-derivation>` 。在下例中密钥表示  AES-256-CBC + HMACSHA256 加密机，并且有效载荷可以进一步进行细分：
  * 128 位的密钥修正。
  * 128 位的初始化向量。
  * 48 位的 AES-256-CBC 输出。
  * HMACSHA256 身份验证标签。
  
A sample protected payload is illustrated below.

以下图为例。

.. literalinclude:: authenticated-encryption-details/_static/protectedpayload.txt
        :linenos:

From the payload format above the first 32 bits, or 4 bytes are the magic header identifying the version (09 F0 C9 F0)

载荷格式的头 32 位（4 字节）是 magic header，标识了版本 (09 F0 C9 F0)

The next 128 bits, or 16 bytes is the key identifier (80 9C 81 0C 19 66 19 40 95 36 53 F8 AA FF EE 57)

随后的 128 位（16 字节）是密钥识别码 (80 9C 81 0C 19 66 19 40 95 36 53 F8 AA FF EE 57)

The remainder contains the payload and is specific to the format used.

余下的部分包含了负载和特定的使用格式。


.. WARNING::
  All payloads protected to a given key will begin with the same 20-byte (magic value, key id) header. Administrators can use this fact for diagnostic purposes to approximate when a payload was generated. For example, the payload above corresponds to key {0c819c80-6619-4019-9536-53f8aaffee57}. If after checking the key repository you find that this specific key's activation date was 2015-01-01 and its expiration date was 2015-03-01, then it is reasonable to assume that the payload (if not tampered with) was generated within that window, give or take a small fudge factor on either side.

.. WARNING::
  给定密钥的所有受保护载荷都以相同的 20 字节的头（magic value 和密钥 ID）开始。管理员可以利用这一点在载荷被生成时做个大概的检测。比方说，负载所对应的密钥是 {0c819c80-6619-4019-9536-53f8aaffee57}。如果检查密钥库后发现这个特定密钥的激活时间是 2015-01-01，过期时间是 2015-03-01，那么有理由来假设载荷（如果未被算改过）生成于该窗口内，可以在任意一边提供或接收一个小小的经验值。