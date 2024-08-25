using Microsoft.EntityFrameworkCore;
using VideoMatrix.Data;
using VideoMatrix.Models;

namespace VideoMatrix.Services
{
    public class DeviceService
    {
        private readonly VideoMatrixDbContext _context;

        public DeviceService(VideoMatrixDbContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<List<Transmitter>> GetTransmittersAsync()
        {
            return await _context.Devices.OfType<Transmitter>().ToListAsync();
        }

        public async Task<List<Receiver>> GetReceiversAsync()
        {
            return await _context.Devices.OfType<Receiver>().ToListAsync();
        }

        public async Task AddDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeviceAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }
    }
}
