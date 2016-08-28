Key Immutability and Changing Settings
======================================

密钥不可变性与更新配置
======================================

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

Once an object is persisted to the backing store, its representation is forever fixed. New data can be added to the backing store, but existing data can never be mutated. The primary purpose of this behavior is to prevent data corruption.

一旦对象被持久化到后备存储器，则意味着这将永不可改变。新数据可以添加到后备存储器，但已存在的数据却不会被改变。这么做的主要目的是为了防止数据被破坏。

One consequence of this behavior is that once a key is written to the backing store, it is immutable. Its creation, activation, and expiration dates can never be changed, though it can revoked by using IKeyManager. Additionally, its underlying algorithmic information, master keying material, and encryption at rest properties are also immutable.

这么做的一个结果是当密钥写入后备存储器，它将是不可改变的。它的建立、激活和过期时间都不可变更，尽管可以通过 IKeyManager 撤销。此外，在其底层的算法信息中，主要的加密材料以及其余属性的加密也均是不可改的。

If the developer changes any setting that affects key persistence, those changes will not go into effect until the next time a key is generated, either via an explicit call to IKeyManager.CreateNewKey or via the data protection system's own :ref:`automatic key generation <data-protection-implementation-key-management>` behavior. The settings that affect key persistence are as follows:

如果开发者更新了任何会影响密钥持久性的设置时，这些更新不会立即生效，而是等到下次密钥生成时才生效（要么是通过显式调用 IKeyManager.CreateNewKey，要么是通过保护系统自有的 :ref:`自动生成密钥 <data-protection-implementation-key-management>` 行为）。影响密钥持久性的设置有：

* :ref:`The default key lifetime <data-protection-implementation-key-management>`
* :ref:`默认密钥的生命周期 <data-protection-implementation-key-management>`
* :ref:`The key encryption at rest mechanism <data-protection-implementation-key-encryption-at-rest>`
* :ref:`密钥加密的剩余机制 <data-protection-implementation-key-encryption-at-rest>`
* :ref:`The algorithmic information contained within the key <data-protection-changing-algorithms>`
* :ref:`密钥所包含的算法信息 <data-protection-changing-algorithms>`

If you need these settings to kick in earlier than the next automatic key rolling time, consider making an explicit call to IKeyManager.CreateNewKey to force the creation of a new key. Remember to provide an explicit activation date ({ now + 2 days } is a good rule of thumb to allow time for the change to propagate) and expiration date in the call.

如果你需要这些设置在下次自动启用前发挥作用，可以考虑显式调用 IKeyManager.CreateNewKey 来强制创建一个新的密钥。谨记要提供一个明确的调用的激活时间（比如 { now + 2 days } 就是个不错的经验值，以适应变化传播时间）和过期时间。

.. TIP::
  All applications touching the repository should specify the same settings with the IDataProtectionBuilder extension methods, otherwise the properties of the persisted key will be dependent on the particular application that invoked the key generation routines.
  
.. TIP::
  连接同一仓库的所有应用程序在调用 ConfigureDataProtection 中必须指定相同的配置，不然持久化密钥的属性将依赖于调用密钥创建程序的特定应用程序