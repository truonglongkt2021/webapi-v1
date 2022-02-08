﻿using StreamFile.Core.Utils;
using Invedia.Data.EF.Models;
using System;

namespace StreamFile.Contract.Repository.Models
{
    public abstract class Entity : StringEntity
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");

            CreatedTime = LastUpdatedTime = CoreHelper.SystemTimeNow;
        }
    }
}