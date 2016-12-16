using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using PartyServer.DAL;
using PartyServer.Model;
using Swashbuckle.Swagger.Annotations;
using AutoMapper;

namespace PartyServer.Controllers
{
    [RoutePrefix("api/parties/invitations")]
    public class InvitationsController : ApiController
    {
        private readonly TimeSpan InvitationTtl = TimeSpan.FromMinutes(1);

        private readonly PartyContext _partyContext;

        public InvitationsController()
        {
            _partyContext = new PartyContext();

            Mapper.Initialize(cfg => cfg.CreateMap<DomainModel.Invitation, Invitation>().ReverseMap());
        }

        [HttpGet]
        [Route("{invitation_id}", Name = "GetInvitation")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Invitation))]
        public async Task<IHttpActionResult> GetInvitationAsync([FromUri(Name = "invitation_id")] int invitationId)
        {
            var invite = await _partyContext.Invitations.SingleOrDefaultAsync(i => i.Id == invitationId);
            if (invite == null)
            {
                return NotFound();
            }

            var invitation = Mapper.Map<DomainModel.Invitation, Invitation>(invite);
            return Ok(invitation);
        }

        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        public async Task<IHttpActionResult> InviteAsync([FromBody] Invitation invitation)
        {
            var expiredInvitation = await _partyContext.Invitations.SingleOrDefaultAsync(
                i => i.InitiatorId == invitation.InitiatorId && DateTime.Now > i.ExpirationTime);
            if (expiredInvitation != null)
            {
                _partyContext.Invitations.Remove(expiredInvitation);
            }

            var invite = Mapper.Map<Invitation, DomainModel.Invitation>(invitation);
            invite.ExpirationTime = DateTime.UtcNow.Add(InvitationTtl);
            var addedInvite = _partyContext.Invitations.Add(invite);
            try
            {
                await _partyContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            var uri = Url.Link("GetInvitation", new { invitation_id = addedInvite.Id });
            return Created(uri, addedInvite);
        }

        [HttpPost]
        [Route("{invitation_id}/accepted")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult AcceptInvitation([FromUri(Name = "invitation_id")] int invitationId)
        {
            return Ok();
        }

        [HttpPost]
        [Route("{invitation_id}/rejected")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> RejectInvitation([FromUri(Name = "invitation_id")] int invitationId)
        {
            var invite = await _partyContext.Invitations.SingleOrDefaultAsync(i => i.Id == invitationId);
            if (invite == null)
            {
                return NotFound();
            }

            _partyContext.Invitations.Remove(invite);

            var rowCount = await _partyContext.SaveChangesAsync();
            if (rowCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}