using System.Diagnostics.CodeAnalysis;

namespace Hookr.Web.Backend.Utilities.Jwt
{
    public class JwtPayloadReadResult
    {
        public bool Success { get; set; }
        
        public JwtPayload Payload { get; set; }
    }
}