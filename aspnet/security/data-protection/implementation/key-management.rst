.. _data-protection-implementation-key-management:

Key Management
==============

密钥管理
==============

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

The data protection system automatically manages the lifetime of master keys used to protect and unprotect payloads. Each key can exist in one of four stages.

* Created - the key exists in the key ring but has not yet been activated. The key shouldn't be used for new Protect operations until sufficient time has elapsed that the key has had a chance to propagate to all machines that are consuming this key ring.
* Active - the key exists in the key ring and should be used for all new Protect operations.
* Expired - the key has run its natural lifetime and should no longer be used for new Protect operations.
* Revoked - the key is compromised and must not be used for new Protect operations.

数据保护系统会自动管理主密钥的生命周期。每一个密钥都存在四种阶段。

*创建 - 密钥存在于密钥环但尚未被激活。在密钥传播到所有正在消费该密钥环的设备（的充足时间）之前，密钥不会被用于新的 Protect 操作，
*活动 - 密钥存在于密钥环，并将用于所有新 Protect 操作。
*过期 - 密钥已超过其寿命，应该不会再被用于新的 Protect 操作。
*吊销 - 密钥被泄露，肯定不会再被用于新的 Protect 操作。

Created, active, and expired keys may all be used to unprotect incoming payloads. Revoked keys by default may not be used to unprotect payloads, but the application developer can :ref:`override this behavior <data-protection-consumer-apis-dangerous-unprotect>` if necessary.

刚创建的密钥、处于活动状态的密钥记忆过期的密钥都可被用来取消保护有效载荷。被吊销的密钥一般来讲不会再被使用，但如有必要应用程序开发者可以 :ref:`覆盖该行为 <data-protection-consumer-apis-dangerous-unprotect>`

.. WARNING::
  The developer might be tempted to delete a key from the key ring (e.g., by deleting the corresponding file from the file system). At that point, all data protected by the key is permanently undecipherable, and there is no emergency override like there is with revoked keys. Deleting a key is truly destructive behavior, and consequently the data protection system exposes no first-class API for performing this operation.

.. WARNING::
  开发人员或许会试图从密钥环中删除密钥（比方说通过从文件系统中删除相应的文件）。这样一来，所有受此密钥保护的数据将永不可被识别，且没有像被吊销的密钥那样的应急覆盖方法。删除密钥实际上是非常具有破坏性的行为，因此数据保护系统所暴露的第一梯队 API 接口中没有响应的接口来执行该操作。 

Default key selection
---------------------

默认密钥的选择
---------------------

When the data protection system reads the key ring from the backing repository, it will attempt to locate a "default" key from the key ring. The default key is used for new Protect operations.

当数据保护系统从后备库中读到密钥环，它会试图从密钥环中找到一个「默认的」密钥。这个默认的密钥会被用户新的 Protect 操作。

The general heuristic is that the data protection system chooses the key with the most recent activation date as the default key. (There's a small fudge factor to allow for server-to-server clock skew.) If the key is expired or revoked, and if the application has not disabled automatic key generation, then a new key will be generated with immediate activation per the :ref:`key expiration and rolling <data-protection-implementation-key-management-expiration>` policy below.

一般数据保护系统会选择激活时间最近的密钥作为默认密钥。（一些经验值可以修正服务器对服务器的时间偏差。）如果密钥过期或撤销，同时应用程序未禁用自动生成密钥，则新密钥将生成并根据下述 :ref:`密钥过期与滚动启用 <data-protection-implementation-key-management-expiration>` 策略立即生效

The reason the data protection system generates a new key immediately rather than falling back to a different key is that new key generation should be treated as an implicit expiration of all keys that were activated prior to the new key. The general idea is that new keys may have been configured with different algorithms or encryption-at-rest mechanisms than old keys, and the system should prefer the current configuration over falling back.

数据保护系统之所以会立即生成一个新密钥而不是回滚到另一个密钥，是因为新生成的密钥后，将显式地把所有激活时间早于新密钥的密钥全部处理为过期状态。总的来说新密钥已配置成不同的算法或其它加密机制，与老密钥是不同的，而且系统会优先使用当前配置（而不是之前的）。

There is an exception. If the application developer has :ref:`disabled automatic key generation <data-protection-configuring-disable-automatic-key-generation>`, then the data protection system must choose something as the default key. In this fallback scenario, the system will choose the non-revoked key with the most recent activation date, with preference given to keys that have had time to propagate to other machines in the cluster. The fallback system may end up choosing an expired default key as a result. The fallback system will never choose a revoked key as the default key, and if the key ring is empty or every key has been revoked then the system will produce an error upon initialization.

当然也有例外。如果应用程序开发人员 :ref:`禁用了密钥自动生成 <data-protection-configuring-disable-automatic-key-generation>` ，那么数据保护系统就必然会选择某项作为默认密钥。在此回退情况下，系统将选择激活时间最新的、有足够时间传播到集群其他设备中的未撤销密钥。回退系统可能最终会选择一个已过期的默认密钥。回退系统不会选择已被撤销的密钥作为默认密钥，同时如果密钥环为空或所有密钥均被撤销，则系统将在初始化时产生错误。

.. _data-protection-implementation-key-management-expiration:

Key expiration and rolling
--------------------------

密钥的过期与启用
--------------------------

When a key is created, it is automatically given an activation date of { now + 2 days } and an expiration date of { now + 90 days }. The 2-day delay before activation gives the key time to propagate through the system. That is, it allows other applications pointing at the backing store to observe the key at their next auto-refresh period, thus maximizing the chances that when the key ring does become active it has propagated to all applications that might need to use it.

新创建的密钥会被自动分配激活时间为 { now + 2 days } 以及过期时间为 { now + 90 days } 。前两天的延迟激活时间给密钥在全系统范围内传播提供了时间。也就是说，它允许其他应用程序到后备存储库中在密钥下次自动更新时对其观察，从而在密钥环激活密钥时尽可能地提高它传播到其它所有需要它的应用程序中的几率。

If the default key will expire within 2 days and if the key ring does not already have a key that will be active upon expiration of the default key, then the data protection system will automatically persist a new key to the key ring. This new key has an activation date of { default key's expiration date } and an expiration date of { now + 90 days }. This allows the system to automatically roll keys on a regular basis with no interruption of service.

如果默认密钥将在两天内到期，并且密钥环在该时间点之前没有可用的激活了的默认密钥，数据保护系统会自动生成新的密钥并持久到密钥环中。该新密钥的激活时间是 { default key's expiration date } ，过期时间则是 { now + 90 days } 。这使得系统能有规律地为未中断的服务启用密钥。

There might be circumstances where a key will be created with immediate activation. One example would be when the application hasn't run for a time and all keys in the key ring are expired. When this happens, the key is given an activation date of { now } without the normal 2-day activation delay.

有一种场景，就是密钥创建后立即激活。举个例子，当应用程序有一段时间没有运行了，密钥环内的所有密钥都会过期。当发生这种情况时，给定密钥的激活时间通常是 { now } 的时间而不是延迟两天激活。

The default key lifetime is 90 days, though this is configurable as in the following example.

默认密钥的生命周期是九十天，尽管这可以通过配置来改变，如下例。

.. code-block:: c#

  services.ConfigureDataProtection(configure =>
  {
      // use 14-day lifetime instead of 90-day lifetime
      configure.SetDefaultKeyLifetime(TimeSpan.FromDays(14));
  });

An administrator can also change the default system-wide, though an explicit call to SetDefaultKeyLifetime will override any system-wide policy. The default key lifetime cannot be shorter than 7 days.

管理员也可以改变默认的系统范围，虽然显式调用 SetDefaultKeyLifetime 将会覆盖任何系统范围的政策。默认密钥的生命周期不少于七天。

Automatic keyring refresh
-------------------------

自动刷新密钥环
-------------------------

When the data protection system initializes, it reads the key ring from the underlying repository and caches it in memory. This cache allows Protect and Unprotect operations to proceed without hitting the backing store. The system will automatically check the backing store for changes approximately every 24 hours or when the current default key expires, whichever comes first.

在数据保护系统初始化时，它会从底层的密钥库中读取密钥环并将之缓存于内存之中。这一缓存允许对其进行 Protect 和 Unprotect 操作而不会影响到后备存储器。系统将大约每 24 小时或当前默认密钥过期时自动检查后备存储器，以先到者为准。

.. WARNING::
  Developers should very rarely (if ever) need to use the key management APIs directly. The data protection system will perform automatic key management as described above.

.. WARNING::
  开发者鲜有需要（如果有的话）直接使用密钥管理 APIs 的，数据保护系统会自动执行上述密钥管理。

The data protection system exposes an interface IKeyManager that can be used to inspect and make changes to the key ring. The DI system that provided the instance of IDataProtectionProvider can also provide an instance of IKeyManager for your consumption. Alternatively, you can pull the IKeyManager straight from the IServiceProvider as in the example below.

数据保护系统向外公开了 IKeyManager 接口，通过它可以检查和更改密钥环。提供了 IDataProtectionProvider 的 DI 系统也将提供可供消费的 IKeyManager 实例。另外，你可从 IServiceProvider 中直接拉取 IKeyManager，如下例所示。

Any operation which modifies the key ring (creating a new key explicitly or performing a revocation) will invalidate the in-memory cache. The next call to Protect or Unprotect will cause the data protection system to reread the key ring and recreate the cache.

任何修改密钥环的操作（显式创建新密钥或执行摧毁密钥）都将置内存缓存中的密钥为无效。下次调用 Protect 或 Unprotect 方法时数据保护系统将重新读取密钥环并重建缓存。

The sample below demonstrates using the IKeyManager interface to inspect and manipulate the key ring, including revoking existing keys and generating a new key manually.

下例演示了使用 IKeyManager 接口来检查和操作密钥环，包括撤销现有的密钥以及手工生成新密钥。

.. literalinclude:: key-management/samples/key-management.cs
        :language: c#
        :linenos:

Key storage
-----------

密钥存储
-----------

The data protection system has a heuristic whereby it tries to deduce an appropriate key storage location and encryption at rest mechanism automatically. This is also configurable by the app developer. The following documents discuss the in-box implementations of these mechanisms:

数据保护系统具有启发式的功能，它会尝试自动推断其它的密钥存储与加密机制。这也是应用程序开发者可配置的。下列文档讨论了这些机制的内部实现。

* :ref:`In-box key storage providers <data-protection-implementation-key-storage-providers>`
* :ref:`密钥存储内部提供程序 <data-protection-implementation-key-storage-providers>`
* :ref:`In-box key encryption at rest providers <data-protection-implementation-key-encryption-at-rest-providers>`
* :ref:`其它密钥加密内部提供程序 <data-protection-implementation-key-encryption-at-rest-providers>`

