using System;
using System.Collections.Generic;
using System.Text;
using SecureDelete;
using SecureDelete.Actions;

namespace BridgeExample {
    [Bridge(Name = "PluginBridge", Exposed = true)]
    public class ExampleBridge : IBridge {
        #region Constants

        private static string BridgeName = "ExampleBridge";

        #endregion

        #region  Properties

        public string Name {
            get { return BridgeName; }
        }

        private WipeSession _session;
        public SecureDelete.WipeSession Session {
            get { return _session; }
            set { _session = value; }
        }

        private bool _afterWipe;
        public bool AfterWipe {
            get { return _afterWipe; }
            set { _afterWipe = value; }
        }

        #endregion

        #region Constructor

        public ExampleBridge() {

        }

        public ExampleBridge(WipeSession session, bool afterWipe) {
            _session = session;
            _afterWipe = afterWipe;
        }

        #endregion

        #region Public methods

        [BridgeMember(Name = "ExampleBridgeMethod", Signature = "public void ExampleBridgeMethod(string message)", 
                      Description = "Example of a methods that accepts a string parameter.")]
        [BridgeMemberParameter(Name = "message", Description = "The message to be returned by the method.")]
        public string PluginExposedMethod(string message) {
            return "ExampleBridgeMethod received the following string: " + message;
        }

        #endregion

        public bool TestMode {
            get { return false; }
            set { }
        }

        public BridgeLogger Logger {
            get { return null; }
            set { }
        }

        public void Open() {
            // initialization code could be added here...
        }

        public void Close() {
            // termination code could be added here...
        }
    }
}
