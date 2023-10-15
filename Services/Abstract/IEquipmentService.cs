﻿using Eassistance.Domain;
namespace Eassistance.Services.Abstract
{
    public interface IEquipmentService
    {
        Task<List<Equipment>> GetAllEquipments(Unit unit);
        Task<bool> CreateEquipment(Equipment equipment);
        Task<Equipment> GetEquipmentById(Guid id);
        Task<bool> DeleteEquipment(Equipment equipment);
    }
}
