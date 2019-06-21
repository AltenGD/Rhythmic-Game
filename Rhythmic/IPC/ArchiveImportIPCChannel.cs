using osu.Framework.Platform;
using Rhythmic.Database;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhythmic.IPC
{
    public class ArchiveImportIPCChannel : IpcChannel<ArchiveImportMessage>
    {
        private readonly ICanAcceptFiles importer;

        public ArchiveImportIPCChannel(IIpcHost host, ICanAcceptFiles importer = null)
            : base(host)
        {
            this.importer = importer;
            MessageReceived += msg =>
            {
                Debug.Assert(importer != null);
                ImportAsync(msg.Path).ContinueWith(t =>
                {
                    if (t.Exception != null) throw t.Exception;
                }, TaskContinuationOptions.OnlyOnFaulted);
            };
        }

        public async Task ImportAsync(string path)
        {
            if (importer == null)
            {
                //we want to contact a remote osu! to handle the import.
                await SendMessageAsync(new ArchiveImportMessage { Path = path });
                return;
            }

            if (importer.HandledExtensions.Contains(Path.GetExtension(path)?.ToLowerInvariant()))
                importer.Import(path);
        }
    }

    public class ArchiveImportMessage
    {
        public string Path;
    }
}
