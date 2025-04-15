using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotoLocadora.Application.Features.Images.Dto;


public record UploadImageDto([Required] IFormFile File);
