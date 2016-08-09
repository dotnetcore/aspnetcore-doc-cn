.. _data-protection-extensibility-core-crypto:

Core cryptography extensibility
===============================

.. include:: thread-safety-included.txt

.. _data-protection-extensibility-core-crypto-iauthenticatedencryptor:

IAuthenticatedEncryptor
-----------------------

The **IAuthenticatedEncryptor** interface is the basic building block of the cryptographic subsystem. There is generally one IAuthenticatedEncryptor per key, and the IAuthenticatedEncryptor instance wraps all cryptographic key material and algorithmic information necessary to perform cryptographic operations.

As its name suggests, the type is responsible for providing authenticated encryption and decryption services. It exposes the following two APIs.

* Decrypt(ArraySegment<byte> ciphertext, ArraySegment<byte> additionalAuthenticatedData) : byte[]
* Encrypt(ArraySegment<byte> plaintext, ArraySegment<byte> additionalAuthenticatedData) : byte[]

The Encrypt method returns a blob that includes the enciphered plaintext and an authentication tag. The authentication tag must encompass the additional authenticated data (AAD), though the AAD itself need not be recoverable from the final payload. The Decrypt method validates the authentication tag and returns the deciphered payload. All failures (except ArgumentNullException and similar) should be homogenized to CryptographicException.

.. note:: 
  The IAuthenticatedEncryptor instance itself doesn't actually need to contain the key material. For example, the implementation could delegate to an HSM for all operations.

.. _data-protection-extensibility-core-crypto-iauthenticatedencryptordescriptor:

IAuthenticatedEncryptorDescriptor
---------------------------------

The **IAuthenticatedEncryptorDescriptor** interface represents a type that knows how to create an :ref:`IAuthenticatedEncryptor <data-protection-extensibility-core-crypto-iauthenticatedencryptor>` instance. Its API is as follows.

* CreateEncryptorInstance() : IAuthenticatedEncryptor
* ExportToXml() : XmlSerializedDescriptorInfo

Like IAuthenticatedEncryptor, an instance of IAuthenticatedEncryptorDescriptor is assumed to wrap one specific key. This means that for any given IAuthenticatedEncryptorDescriptor instance, any authenticated encryptors created by its CreateEncryptorInstance method should be considered equivalent, as in the below code sample.

.. code-block:: c#

  // we have an IAuthenticatedEncryptorDescriptor instance
  IAuthenticatedEncryptorDescriptor descriptor = ...;

  // get an encryptor instance and perform an authenticated encryption operation
  ArraySegment<byte> plaintext = new ArraySegment<byte>(Encoding.UTF8.GetBytes("plaintext"));
  ArraySegment<byte> aad = new ArraySegment<byte>(Encoding.UTF8.GetBytes("AAD"));
  var encryptor1 = descriptor.CreateEncryptorInstance();
  byte[] ciphertext = encryptor1.Encrypt(plaintext, aad);

  // get another encryptor instance and perform an authenticated decryption operation
  var encryptor2 = descriptor.CreateEncryptorInstance();
  byte[] roundTripped = encryptor2.Decrypt(new ArraySegment<byte>(ciphertext), aad);


  // the 'roundTripped' and 'plaintext' buffers should be equivalent

XML Serialization
-----------------

The primary difference between IAuthenticatedEncryptor and IAuthenticatedEncryptorDescriptor is that the descriptor knows how to create the encryptor and supply it with valid arguments. Consider an IAuthenticatedEncryptor whose implementation relies on SymmetricAlgorithm and KeyedHashAlgorithm. The encryptor's job is to consume these types, but it doesn't necessarily know where these types came from, so it can't really write out a proper description of how to recreate itself if the application restarts. The descriptor acts as a higher level on top of this. Since the descriptor knows how to create the encryptor instance (e.g., it knows how to create the required algorithms), it can serialize that knowledge in XML form so that the encryptor instance can be recreated after an application reset.

.. _data-protection-extensibility-core-crypto-exporttoxml:

The descriptor can be serialized via its ExportToXml routine. This routine returns an XmlSerializedDescriptorInfo which contains two properties: the XElement representation of the descriptor and the Type which represents an :ref:`IAuthenticatedEncryptorDescriptorDeserializer <data-protection-extensibility-core-crypto-iauthenticatedencryptordescriptordeserializer>` which can be used to resurrect this descriptor given the corresponding XElement.

The serialized descriptor may contain sensitive information such as cryptographic key material. The data protection system has built-in support for encrypting information before it's persisted to storage. To take advantage of this, the descriptor should mark the element which contains sensitive information with the attribute name "requiresEncryption" (xmlns "\http://schemas.asp.net/2015/03/dataProtection"), value "true".

.. tip:: 
  There's a helper API for setting this attribute. Call the extension method XElement.MarkAsRequiresEncryption() located in namespace Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.

There can also be cases where the serialized descriptor doesn't contain sensitive information. Consider again the case of a cryptographic key stored in an HSM. The descriptor cannot write out the key material when serializing itself since the HSM will not expose the material in plaintext form. Instead, the descriptor might write out the key-wrapped version of the key (if the HSM allows export in this fashion) or the HSM's own unique identifier for the key.

.. _data-protection-extensibility-core-crypto-iauthenticatedencryptordescriptordeserializer:

IAuthenticatedEncryptorDescriptorDeserializer
---------------------------------------------

The **IAuthenticatedEncryptorDescriptorDeserializer** interface represents a type that knows how to deserialize an IAuthenticatedEncryptorDescriptor instance from an XElement. It exposes a single method:

* ImportFromXml(XElement element) : IAuthenticatedEncryptorDescriptor

The ImportFromXml method takes the XElement that was returned by :ref:`IAuthenticatedEncryptorDescriptor.ExportToXml <data-protection-extensibility-core-crypto-exporttoxml>` and creates an equivalent of the original IAuthenticatedEncryptorDescriptor.

Types which implement IAuthenticatedEncryptorDescriptorDeserializer should have one of the following two public constructors:

* .ctor(IServiceProvider)
* .ctor()

.. note:: 
  The IServiceProvider passed to the constructor may be null.

IAuthenticatedEncryptorConfiguration
------------------------------------

The **IAuthenticatedEncryptorConfiguration** interface represents a type which knows how to create :ref:`IAuthenticatedEncryptorDescriptor <data-protection-extensibility-core-crypto-iauthenticatedencryptordescriptor>` instances. It exposes a single API.

* CreateNewDescriptor() : IAuthenticatedEncryptorDescriptor

Think of IAuthenticatedEncryptorConfiguration as the top-level factory. The configuration serves as a template. It wraps algorithmic information (e.g., this configuration produces descriptors with an AES-128-GCM master key), but it is not yet associated with a specific key.

When CreateNewDescriptor is called, fresh key material is created solely for this call, and a new IAuthenticatedEncryptorDescriptor is produced which wraps this key material and the algorithmic information required to consume the material. The key material could be created in software (and held in memory), it could be created and held within an HSM, and so on. The crucial point is that any two calls to CreateNewDescriptor should never create equivalent IAuthenticatedEncryptorDescriptor instances.

The IAuthenticatedEncryptorConfiguration type serves as the entry point for key creation routines such as :ref:`automatic key rolling <data-protection-implementation-key-management>`. To change the implementation for all future keys, register a singleton IAuthenticatedEncryptorConfiguration in the service container.
