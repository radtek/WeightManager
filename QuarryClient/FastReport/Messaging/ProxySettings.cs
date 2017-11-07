using System;
using System.Collections.Generic;
using System.Text;

namespace FastReport.Messaging
{
    /// <summary>
    /// Represents proxy settings of the messenger.
    /// </summary>
    public class ProxySettings
    {
        #region Fields

        private string server;
        private int port;
        private string username;
        private string password;
        private ProxyType proxyType;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the proxy server.
        /// </summary>
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        /// <summary>
        /// Gets or sets the port number of proxy server.
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Gets or sets the type of proxy.
        /// </summary>
        public ProxyType ProxyType
        {
            get { return proxyType; }
            set { proxyType = value; }
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxySettings"/> class.
        /// </summary>
        /// <param name="server">The proxy server.</param>
        /// <param name="port">The port number of server.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="proxyType">The type of proxy.</param>
        public ProxySettings(string server, int port, string username, string password, ProxyType proxyType)
        {
            this.server = server;
            this.port = port;
            this.username = username;
            this.password = password;
            this.proxyType = proxyType;
        }

        #endregion // Constructors
    }

    /// <summary>
    /// Represents the type of rpoxy.
    /// </summary>
    public enum ProxyType
    {
        /// <summary>
        /// The HTTP proxy type.
        /// </summary>
        Http,

        /// <summary>
        /// The SOCKS4 proxy type.
        /// </summary>
        Socks4,

        /// <summary>
        /// The SOCKS5 proxy type.
        /// </summary>
        Socks5
    }
}
