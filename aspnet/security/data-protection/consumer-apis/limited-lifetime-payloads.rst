Limiting the lifetime of protected payloads
===========================================

There are scenarios where the application developer wants to create a protected payload that expires after a set period of time. For instance, the protected payload might represent a password reset token that should only be valid for one hour. It is certainly possible for the developer to create his own payload format that contains an embedded expiration date, and advanced developers may wish to do this anyway, but for the majority of developers managing these expirations can grow tedious.

To make this easier for our developer audience, the package Microsoft.AspNetCore.DataProtection.Extensions contains utility APIs for creating payloads that automatically expire after a set period of time. These APIs hang off of the ITimeLimitedDataProtector type.

API usage
---------

The ITimeLimitedDataProtector interface is the core interface for protecting and unprotecting time-limited / self-expiring payloads. To create an instance of an ITimeLimitedDataProtector, you'll first need an instance of a regular :doc:`IDataProtector <../consumer-apis/overview>` constructed with a specific purpose. Once the IDataProtector instance is available, call the IDataProtector.ToTimeLimitedDataProtector extension method to get back a protector with built-in expiration capabilities.

ITimeLimitedDataProtector exposes the following API surface and extension methods:

* CreateProtector(string purpose) : ITimeLimitedDataProtector
  This API is similar to the existing IDataProtectionProvider.CreateProtector in that it can be used to create :doc:`purpose chains <purpose-strings>` from a root time-limited protector.

* Protect(byte[] plaintext, DateTimeOffset expiration) : byte[]
* Protect(byte[] plaintext, TimeSpan lifetime) : byte[]
* Protect(byte[] plaintext) : byte[]
* Protect(string plaintext, DateTimeOffset expiration) : string
* Protect(string plaintext, TimeSpan lifetime) : string
* Protect(string plaintext) : string

In addition to the core Protect methods which take only the plaintext, there are new overloads which allow specifying the payload's expiration date. The expiration date can be specified as an absolute date (via a DateTimeOffset) or as a relative time (from the current system time, via a TimeSpan). If an overload which doesn't take an expiration is called, the payload is assumed never to expire.

* Unprotect(byte[] protectedData, out DateTimeOffset expiration) : byte[]
* Unprotect(byte[] protectedData) : byte[]
* Unprotect(string protectedData, out DateTimeOffset expiration) : string
* Unprotect(string protectedData) : string

The Unprotect methods return the original unprotected data. If the payload hasn't yet expired, the absolute expiration is returned as an optional out parameter along with the original unprotected data. If the payload is expired, all overloads of the Unprotect method will throw CryptographicException.

.. warning:: 
  It is not advised to use these APIs to protect payloads which require long-term or indefinite persistence. "Can I afford for the protected payloads to be permanently unrecoverable after a month?" can serve as a good rule of thumb; if the answer is no then developers should consider alternative APIs.

The sample below uses the :doc:`non-DI code paths <../configuration/non-di-scenarios>` for instantiating the data protection system. To run this sample, ensure that you have first added a reference to the Microsoft.AspNetCore.DataProtection.Extensions package.

.. literalinclude:: limited-lifetime-payloads/samples/limitedlifetimepayloads.cs
  :language: none
  :linenos:
