using System;
using ASMR.Core.Entities;

namespace ASMR.Mobile.Services.Events
{
    public class AuthenticationEventArgs : EventArgs
    {
        public AuthenticationEventArgs() : this(null, true)
        {
        }

        public AuthenticationEventArgs(NormalizedUser user) : this (user, false)
        {
        }


        public AuthenticationEventArgs(NormalizedUser user, bool previousState)
        {
            PreviousState = previousState;
            State = user is not null;
            User = user;
        }

        public bool PreviousState { get; }

        public bool State { get; }

        public NormalizedUser User { get; }
    }
}