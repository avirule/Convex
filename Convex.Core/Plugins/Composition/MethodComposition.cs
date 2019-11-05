﻿#region

using System;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace Convex.Core.Plugins.Composition
{
    public class MethodComposition<TEventArgs> : IAsyncComposition<TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        ///     Creates a new instance of MethodRegistrar
        /// </summary>
        /// <param name="method"></param>
        /// <param name="composition">registrable composition to be executed</param>
        /// <param name="description">describes composition</param>
        public MethodComposition(Func<TEventArgs, Task> method, Composition composition, CompositionDescription description = null)
        {
            UniqueId = composition.UniqueId;
            Priority = composition.Priority;
            Commands = composition.Commands;
            Method = method;
            Description = description ?? new CompositionDescription("", "undefined");
        }

        #region MEMBERS
        
        public string UniqueId { get; }
        public int Priority { get; }
        public string[] Commands { get; }
        public Func<TEventArgs, Task> Method { get; }
        public CompositionDescription Description { get; }

        #endregion

        #region METHODS

        public async Task InvokeAsync(TEventArgs args)
        {
            await Method.Invoke(args);
        }

        #endregion
    }
}
