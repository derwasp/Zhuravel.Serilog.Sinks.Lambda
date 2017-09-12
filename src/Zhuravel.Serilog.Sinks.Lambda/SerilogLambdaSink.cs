using System;
using System.IO;

using Amazon.Lambda.Core;

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Lambda
{
    internal sealed class SerilogLambdaSink : ILogEventSink
    {
        readonly ITextFormatter _textFormatter;

        public SerilogLambdaSink(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            using (var renderSpace = new StringWriter())
            {
                _textFormatter.Format(logEvent, renderSpace);
                LambdaLogger.Log(renderSpace.ToString());
            }
        }
    }
}
