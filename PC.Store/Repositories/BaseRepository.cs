namespace PC.Store.Repositories;

public class BaseRepository //: IDisposable
{
    protected readonly PostCrossContext DbContext;

    public BaseRepository(PostCrossContext dbContext) =>
        DbContext = dbContext;

    #region | Dispose pattern implementation |

    //private bool _disposed;

    // protected virtual void Dispose(bool disposing)
    // {
    //     if (!_disposed && disposing)
    //         DbContext.Dispose();
    //
    //     _disposed = true;
    // }
    //
    // public void Dispose()
    // {
    //     Dispose(true);
    // }

    #endregion
}