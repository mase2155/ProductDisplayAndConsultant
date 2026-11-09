using Microsoft.AspNetCore.Http;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// ISession giả lập trong memory (dùng test SessionCartHelper).
/// </summary>
public sealed class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

    public bool IsAvailable => true;

    public string Id { get; } = Guid.NewGuid().ToString();

    public IEnumerable<string> Keys => _store.Keys;

    public void Clear()
    {
        _store.Clear();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task LoadAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        _store.Remove(key);
    }

    public void Set(string key, byte[] value)
    {
        _store[key] = value;
    }

    public bool TryGetValue(string key, out byte[] value)
    {
        return _store.TryGetValue(key, out value!);
    }
}
