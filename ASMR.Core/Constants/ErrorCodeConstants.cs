//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ErrorCodeConstants.cs
//
namespace ASMR.Core.Constants;

public static class ErrorCodeConstants
{
	public const int GenericServerError = 1000;

	public const int EmailProviderUnavailable = 1100;
	public const int EmailSendingFailure = 1101;

	public const int GenericClientError = 2000;

	public const int EndpointNotFound = 2100;
	public const int RequestMethodNotAllowed = 2101;
	public const int RequestMediaTypeNotSupported = 2102;

	public const int RequiredParameterNotProvided = 2200;
	public const int InvalidModelFormat = 2201;
	public const int ModelValidationFailed = 2202;
	public const int CaptchaResponseTokenNotProvided = 2203;
	public const int CaptchaVerificationFailed = 2204;

	public const int ResourceNotFound = 2300;
	public const int ResourceEmpty = 2301;

	public const int InvalidAntiforgeryToken = 2400;
	public const int NotAuthenticated = 2401;
	public const int NotAuthorized = 2402;
	public const int AuthenticationFailed = 2403;
	public const int RequiresTwoFactor = 2404;
	public const int InvalidClientPlatform = 2405;
	public const int ClientPlatformNotAllowed = 2406;

	public const int EmailAddressWaitingConfirmation = 2500;
	public const int AccountWaitingForApproval = 2501;
	public const int AccountWasNotApproved = 2501;
}