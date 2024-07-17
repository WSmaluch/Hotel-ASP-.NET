using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API.Models;

public class ImgurService
{
	private readonly string _clientId;
	private readonly HttpClient _httpClient;

	public ImgurService(IConfiguration configuration)
	{
		_clientId = configuration["ImgurApi:ClientId"];
		_httpClient = new HttpClient();
	}

	public async Task<string> UploadImageAsync(IFormFile imageFile)
	{
		if (imageFile != null && imageFile.Length > 0)
		{
			using (var stream = imageFile.OpenReadStream())
			{
                // Call the method to upload the file to Imgur and return the link
                var imgurClient = new ApiClient(_clientId);
				var imageEndpoint = new ImageEndpoint(imgurClient, _httpClient);
				var imageUpload = await imageEndpoint.UploadImageAsync(stream);
				return imageUpload.Link;
			}
		}
		return null;
	}

}
