Password Hashing
================

The data protection code base includes a package *Microsoft.AspNetCore.Cryptography.KeyDerivation* which contains cryptographic key derivation functions. This package is a standalone component and has no dependencies on the rest of the data protection system. It can be used completely independently. The source exists alongside the data protection code base as a convenience.

The package currently offers a method ``KeyDerivation.Pbkdf2`` which allows hashing a password using the `PBKDF2 algorithm <https://tools.ietf.org/html/rfc2898#section-5.2>`_. This API is very similar to the .NET Framework's existing `Rfc2898DeriveBytes type <https://msdn.microsoft.com/en-us/library/System.Security.Cryptography.Rfc2898DeriveBytes(v=vs.110).aspx>`_, but there are three important distinctions:

#. The ``KeyDerivation.Pbkdf2`` method supports consuming multiple PRFs (currently ``HMACSHA1``, ``HMACSHA256``, and ``HMACSHA512``), whereas the ``Rfc2898DeriveBytes`` type only supports ``HMACSHA1``.
#. The ``KeyDerivation.Pbkdf2`` method detects the current operating system and attempts to choose the most optimized implementation of the routine, providing much better performance in certain cases. (On Windows 8, it offers around 10x the throughput of ``Rfc2898DeriveBytes``.)
#. The ``KeyDerivation.Pbkdf2`` method requires the caller to specify all parameters (salt, PRF, and iteration count). The ``Rfc2898DeriveBytes`` type provides default values for these.

.. literalinclude:: password-hashing/samples/passwordhasher.cs
  :language: none

See the source code for ASP.NET Core Identity's ``PasswordHasher`` type for a real-world use case.
