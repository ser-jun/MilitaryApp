﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IOfficerRepository
    {
        Task AddOfficer(int personnelId, string post);
        Task DeleteOfficer(int idOfficer);
        Task UpdateOfficer(int selectedOfficer, string newPosition);
    }
}
