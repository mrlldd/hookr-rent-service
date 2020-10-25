﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Hookr.Core.Repository.Context.Entities.Base
{
    public class TelegramUser
    {
        // same as in telegram "database"
        public int Id { get; set; } 
        [NotNull]
        public string? Username { get; set; }
        public TelegramUserStates State { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}