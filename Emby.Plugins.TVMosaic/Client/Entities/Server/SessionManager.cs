using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TSoft.TVServer.Entities
{
    /// <summary> Manager for sessions. </summary>
    public class SessionManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.SessionManager class. </summary>
        public SessionManager()
        {
        }

        #endregion

        #region Fields

        /// <summary> The sessions. </summary>
        private readonly List<Session> _Sessions = new List<Session>();

        #endregion

        #region Properties
        /// <summary>
        /// Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="sessionId"> Identifier for the session. </param>
        /// <returns> The indexed item. </returns>
        public Session this[string sessionId]
        {
            get { return this._Sessions.FirstOrDefault(x => string.Compare(x.SessionId, sessionId, false) == 0); }
            set
            {
                var se = this._Sessions.FirstOrDefault(x => string.Compare(x.SessionId, sessionId, false) == 0);
                if (se == null)
                {
                    this._Sessions.Add(value);
                }
                else
                {
                    se.SessionName = value.SessionName;
                    se.SessionNotes = value.SessionNotes;
                    se.UserId = value.UserId;
                    se.UserName = value.UserName;
                    se.DeviceId = value.DeviceId;
                    se.DeviceName = value.DeviceName;
                    se.ClientId = value.ClientId;
                    se.ClientName = value.ClientName;
                    se.ClientIP = value.ClientIP;
                    se.ClientMac = value.ClientMac;
                    se.StreamId = value.StreamId;
                    se.StreamName = value.StreamName;
                    se.StreamSource = value.StreamSource;
                    se.StreamUrl = value.StreamUrl;
                    se.StreamStartTime = value.StreamStartTime;
                    se.StreamEndTime = value.StreamEndTime;
                    se.StreamOpen = value.StreamOpen;
                    se.StreamHandle = value.StreamHandle;
                    se.UpdatedOn = DateTime.Now;
                }
            }
        }

        #endregion

        #region Public Methods
        /// <summary> Adds a session. </summary>
        /// <param name="sessionId"> Identifier for the session. </param>
        /// <returns> A Session. </returns>
        public Session AddSession(string sessionId)
        {
            Session se = new Session { SessionId = sessionId };
            this._Sessions.Add(se);
            return se;
        }

        /// <summary> Adds a session. </summary>
        /// <param name="session"> The session. </param>
        public void AddSession(Session session)
        {
            this._Sessions.Add(session);
        }

        /// <summary> Gets a session. </summary>
        /// <param name="sessionId"> Identifier for the session. </param>
        /// <returns> The session. </returns>
        public Session GetSession(string sessionId)
        {
            return this[sessionId];
        }

        /// <summary> Gets the sessions. </summary>
        /// <returns> The sessions. </returns>
        public List<Session> GetSessions()
        {
            return this._Sessions;
        }

        /// <summary> Removes all session. </summary>
        public void RemoveAllSession()
        {
            this._Sessions.Clear();
        }

        /// <summary> Removes the session described by sessionId. </summary>
        /// <param name="session"> The session. </param>
        public void RemoveSession(Session session)
        {
            this._Sessions.Remove(session);
        }

        /// <summary> Removes the session described by sessionId. </summary>
        /// <param name="sessionId"> Identifier for the session. </param>
        public void RemoveSession(string sessionId)
        {
            var se = this[sessionId];
            if (se != null)
                this._Sessions.Remove(se);
        }

        #endregion

    }
}
