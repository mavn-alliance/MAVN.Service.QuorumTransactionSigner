using System;
using JetBrains.Annotations;

namespace MAVN.Service.QuorumTransactionSigner.Client.Exceptions
{
    /// <summary>
    ///    WalletNotFoundException class.
    /// </summary>
    [PublicAPI]
    public class WalletNotFoundException : Exception
    {
        /// <summary>
        ///    WalletNotFoundException constructor.
        /// </summary>
        public WalletNotFoundException(
            string address)
            : base($"Wallet with the specified [{address}] address has not been found.")
        {
            Address = address;
        }
        
        /// <summary>
        ///    Address of a wallet, that has not been found.
        /// </summary>
        public string Address { get; }
    }
}
