using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using PartyServer.DAL;
using PartyServer.Model;
using Swashbuckle.Swagger.Annotations;

namespace PartyServer.Controllers
{
    [RoutePrefix("api/invitations")]
    public class InvitationsController : ApiController
    {
        private readonly PartyContext _partyContext;

        public InvitationsController()
        {
            _partyContext = new PartyContext();
        }

        [HttpGet]
        [Route("{invitation_id}")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Invitation))]
        public async Task<IHttpActionResult> GetInvitationAsync([FromUri(Name = "invitation_id")] int invitationId)
        {
            var invite = await _partyContext.Invitations.FirstOrDefaultAsync(i => i.Id == invitationId);
            if (invite == null)
            {
                return NotFound();
            }

            var invitation = new Invitation
            {
                InitiatorId = invite.InitiatorId,
                TargetId = invite.TargetId
            };

            return Ok(invitation);
        }

        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> InviteAsync([FromBody] Invitation invitation)
        {
            var invite = new DomainModel.Invitation
            {
                InitiatorId = invitation.InitiatorId,
                TargetId = invitation.TargetId
            };
            _partyContext.Invitations.Add(invite);
            int i = await _partyContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{invitation_id}/accepted")]
        public IHttpActionResult AcceptInvitation([FromUri(Name = "invitation_id")] ulong invitationId)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{invitation_id}/rejected")]
        public IHttpActionResult RejectInvitation([FromUri(Name = "invitation_id")] ulong invitationId)
        {
            return Ok();
        }
    }
}