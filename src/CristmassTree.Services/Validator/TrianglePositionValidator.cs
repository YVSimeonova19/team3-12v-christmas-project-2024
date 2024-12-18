using System.Net.Http;
using System.Threading.Tasks;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class TrianglePositionValidator : LightValidator
    {
        private readonly HttpClient httpClient;

        public TrianglePositionValidator(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public override async Task<bool> ValidateLightAsync(Light light)
        {
            bool isInTriangle = IsPointInTriangle(
                light.X, light.Y,
                0.00, 170.30,
                125.80, 170.30,
                62.80, 14.90);

            if (!isInTriangle)
            {
                return false;
            }

            var response = await this.ValidateNext(light);
            if (!response)
            {
                return false;
            }

            return true; // If all checks pass, return true
        }

        // Function to check if the point is inside the triangle using Barycentric coordinates
        private bool IsPointInTriangle(double px, double py,
            double x1, double y1,
            double x2, double y2,
            double x3, double y3)
        {
            double denominator = ((y2 - y3) * (x1 - x3)) + ((x3 - x2) * (y1 - y3));
            double a = (((y2 - y3) * (px - x3)) + ((x3 - x2) * (py - y3))) / denominator;
            double b = (((y3 - y1) * (px - x3)) + ((x1 - x3) * (py - y3))) / denominator;
            double c = 1 - a - b;

            return a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1;
        }
    }
}
