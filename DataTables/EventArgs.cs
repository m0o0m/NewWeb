using System;
using System.Collections.Generic;

namespace DataTables
{
    /// <summary>
    /// Arguments for the 'PreCreate' Editor event
    /// </summary>
    public class PreCreateEventArgs : EventArgs
    {
        /// <summary>
        /// Editor instance that triggered the event
        /// </summary>
        public Editor Editor;

        /// <summary>
        /// Values submitted to the server by the client
        /// </summary>
        public Dictionary<string, object> Values;
    }


    /// <summary>
    /// Arguments for the 'PostCreate' Editor event
    /// </summary>
    public class PostCreateEventArgs : PreCreateEventArgs
    {
        /// <summary>
        /// Newly created row id
        /// </summary>
        public object Id;

        /// <summary>
        /// Data for the new row, as read from the database
        /// </summary>
        public Dictionary<string, object> Data;
    }


    /// <summary>
    /// Arguments for the 'PreEdit' Editor event
    /// </summary>
    public class PreEditEventArgs : EventArgs
    {
        /// <summary>
        /// Editor instance that triggered the event
        /// </summary>
        public Editor Editor;

        /// <summary>
        /// Id of the row to be edited
        /// </summary>
        public object Id;

        /// <summary>
        /// Values submitted to the server by the client
        /// </summary>
        public Dictionary<string, object> Values;
    }


    /// <summary>
    /// Arguments for the 'PostEdit' event
    /// </summary>
    public class PostEditEventArgs : PreEditEventArgs
    {
        /// <summary>
        /// Data for the edited row, as read from the database
        /// </summary>
        public Dictionary<string, object> Data;
    }


    /// <summary>
    /// Arguments for the 'PreRemove' Editor event
    /// </summary>
    public class PreRemoveEventArgs : EventArgs
    {
        /// <summary>
        /// Editor instance that triggered the event
        /// </summary>
        public Editor Editor;

        /// <summary>
        /// Id of the row to be removed
        /// </summary>
        public object Id;

        /// <summary>
        /// Values submitted to the server by the client
        /// </summary>
        public Dictionary<string, object> Values;
    }


    /// <summary>
    /// Arguments for the 'PostRemove' Editor event
    /// </summary>
    public class PostRemoveEventArgs : PreRemoveEventArgs
    { }
}
