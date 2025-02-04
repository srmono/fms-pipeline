using Microsoft.EntityFrameworkCore;
using fleetmanagement.config;
using fleetmanagement.entities;

namespace fleetmanagement.repositories;

public class TruckRepository : ITruckRepository
{
    private readonly TruckDbContext _context;

    public TruckRepository(TruckDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Truck>> GetAllTrucksAsync() => await _context.Trucks.ToListAsync();

    public async Task<Truck?> GetTruckByIdAsync(int id) => await _context.Trucks.FindAsync(id);

    public async Task<Truck> AddTruckAsync(Truck truck)
    {
        _context.Trucks.Add(truck);
        await _context.SaveChangesAsync();
        return truck;
    }

    public async Task<Truck?> UpdateTruckAsync(Truck truck)
    {
        var existingTruck = await _context.Trucks.FindAsync(truck.Id);
        if (existingTruck == null) return null;

        existingTruck.Name = truck.Name;
        existingTruck.Model = truck.Model;
        existingTruck.Year = truck.Year;

        await _context.SaveChangesAsync();
        return existingTruck;
    }

    public async Task<bool> DeleteTruckAsync(int id)
    {
        var truck = await _context.Trucks.FindAsync(id);
        if (truck == null) return false;

        _context.Trucks.Remove(truck);
        await _context.SaveChangesAsync();
        return true;
    }
}
