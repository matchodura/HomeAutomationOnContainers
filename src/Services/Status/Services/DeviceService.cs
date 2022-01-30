﻿using Status.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Status.API.Services;

public class DeviceService
{
    private readonly IMongoCollection<Device> _deviceCollection;

    public DeviceService(
        IOptions<DeviceDatabaseSettings> deviceDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            deviceDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            deviceDatabaseSettings.Value.DatabaseName);

        _deviceCollection = mongoDatabase.GetCollection<Device>(
            deviceDatabaseSettings.Value.DeviceCollectionName);
    }

    public async Task<List<Device>> GetAsync() =>
        await _deviceCollection.Find(_ => true).ToListAsync();

    public async Task<Device?> GetAsync(string id) =>
        await _deviceCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Device newDevice) =>
        await _deviceCollection.InsertOneAsync(newDevice);

    public async Task UpdateAsync(string id, Device updatedDevice) =>
        await _deviceCollection.ReplaceOneAsync(x => x.Id == id, updatedDevice);

    public async Task RemoveAsync(string id) =>
        await _deviceCollection.DeleteOneAsync(x => x.Id == id);
}