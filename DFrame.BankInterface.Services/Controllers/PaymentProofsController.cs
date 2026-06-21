using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using System.Net;
using ExpenseTracker.Services.Utilities;
using System.Security.Claims;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Repository;


namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/paymentproofs")]
    [ApiController]
    public class PaymentProofsController : ControllerBase
    {
        private readonly IPaymentProofService _service;
        
        private ResponseDto _responseDto;
        public PaymentProofsController(IPaymentProofService service, ResponseDto responseDto)
        {
            _service = service;
            _responseDto = responseDto;
        }

        /// <summary>
        /// Creates a new payment proof record.
        /// </summary>
        /// <param name="paymentScheduleGuid">The unique identifier for the payment schedule.</param>
        /// <param name="tref">The transaction reference associated with the payment.</param>
        /// <param name="notes">Additional notes or comments related to the payment proof.</param>
        /// <param name="attachment">The attachment file containing the payment proof document.</param>
        /// <param name="createdBy">The identifier of the user creating the payment proof.</param>
        /// <returns>
        /// An IActionResult indicating the result of the create operation.
        /// - Returns 200 OK with a success message if the payment proof is created successfully.
        /// - Returns 500 Internal Server Error with an error message if there is an exception.
        /// </returns>

        [HttpPost, Route("CreatePaymentProof")]
        public async Task<IActionResult> CreatePaymentProof([FromForm] Guid paymentScheduleGuid, [FromForm] string tref, [FromForm] string notes, [FromForm] IFormFile attachment, [FromForm] string createdBy)
        {
            byte[] compressedAttachment = [];
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return BadRequest(ModelState);
            }
            if (attachment != null && attachment.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await attachment.CopyToAsync(memoryStream);
                    compressedAttachment = AttachmentCompressionHelper.Compress(memoryStream.ToArray());
                }
            }
            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);

            var paymentProofDto = new PaymentProofDto
            {
                PaymentScheduleGuid = paymentScheduleGuid,
                TReference = tref,
                Notes = notes,
                Attachment = compressedAttachment,
                CreatedDate = DateTime.Now,
                CreatedBy = userDetails.UserLoginCode,
                OriginalFileName = attachment.FileName // Store original file name
            };

            _responseDto = await _service.CreatePaymentProofAsync(paymentProofDto);

                return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        /// <summary>
        /// Retrieves all payment proofs associated with a specific payment schedule.
        /// </summary>
        /// <param name="paymentScheduleGuid">The GUID of the payment schedule to filter payment proofs by.</param>
        /// <returns>An IActionResult containing a list of payment proofs and their details. If an error occurs, returns an error message.</returns>
        /// <remarks>
        /// This endpoint allows the retrieval of all payment proofs that are associated with the specified payment schedule GUID. 
        /// Each payment proof includes its details such as ID, transaction reference, notes, created date, created by, and a download URL for the attachment.
        /// </remarks>
        [HttpGet]
        [Route("GetPaymentProofs")]
        public async Task<IActionResult> GetPaymentProofs(Guid paymentScheduleGuid)
        {
            try
            {
                var paymentProofs = await _service.GetPaymentProofsByPaymentScheduleGuidAsync(paymentScheduleGuid);

                if (paymentProofs == null || !paymentProofs.Any())
                {                   
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No payment proofs found.";
                    return StatusCode((int)HttpStatusCode.OK, _responseDto);
                }

                _responseDto.Result = paymentProofs.Select(p => new PaymentProofDto
                {
                    Id = p.Id,
                    PaymentScheduleGuid = p.PaymentScheduleGuid,
                    TReference = p.TReference,
                    Notes = p.Notes,
                    CreatedDate = p.CreatedDate,
                    CreatedBy = p.CreatedBy,
                    OriginalFileName = p.OriginalFileName,
                    DownloadUrl = Url.Action("DownloadAttachment", new { id = p.Id })
                }).ToList();

                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        /// <summary>
        /// Downloads the attachment file associated with a specific payment proof.
        /// </summary>
        /// <param name="id">The ID of the payment proof to retrieve the attachment for.</param>
        /// <returns>An IActionResult containing the file content to be downloaded. If an error occurs, returns an error message.</returns>
        /// <remarks>
        /// This endpoint allows the download of the attachment file associated with a specific payment proof by its ID. 
        /// The file content is decompressed and returned with the appropriate content type and original file name.
        /// </remarks>
        [HttpGet("DownloadAttachment")]
        public async Task<IActionResult> DownloadAttachment(int id)
        {
            try
            {
                // Fetch the payment proof by ID
                var paymentProof = await _service.GetPaymentProofByIdAsync(id);

                if (paymentProof == null)
                {
                    return NotFound("Payment proof not found.");
                }

                // Decompress the file data
                var fileContent = AttachmentCompressionHelper.Decompress(paymentProof.Attachment);
                var fileName = paymentProof.OriginalFileName;
                var contentType = "application/octet-stream"; // Default content type for binary files

 
                var base64FileContent = Convert.ToBase64String(fileContent);
                var response = new
                {
                    FileContent = base64FileContent,
                    FileName = fileName,
                    ContentType = contentType
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

    }
}
