using System.ComponentModel.DataAnnotations;

namespace BookBuddy.API.Models.DTO
{
    public class VerifyPaymentRequestDTO
    { //All three values are mandatory, If even one is missing, We cannot verify the payment and We must reject the request.


        [Required]
        public string RazorpayOrderId { get; set; } = string.Empty;   //It Identifies the order User created.
        [Required]
        public string RazorpayPaymentId { get; set; } = string.Empty; //It Identifies the payment made by the user.{Identifies the completed payment.}
        [Required]
        public string RazorpaySignature { get; set; } = string.Empty; //It is used to verify the authenticity of the payment made by the user. It's a Cryptographic proof from Razorpay that the payment details have not been tampered with
    }
}
