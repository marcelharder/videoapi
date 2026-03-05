using System.Diagnostics.CodeAnalysis;

namespace videoapi.Helpers;
public class VideoParams : IParsable<VideoParams>
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        public int Id { get; set; } 
        public required int Category { get; set; } = 1; // Default to 1 if not provided

        public static VideoParams Parse(string input, IFormatProvider? formatProvider)
    {
        var parts = input.Split('&');
        var videoParams = new VideoParams { Category = 0 }; // Default value for Category

        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2)
            {
                if (keyValue[0].Equals("PageNumber", StringComparison.OrdinalIgnoreCase))
                {
                    videoParams.PageNumber = int.TryParse(keyValue[1], out var pageNumber) ? pageNumber : 1;
                }
                else if (keyValue[0].Equals("PageSize", StringComparison.OrdinalIgnoreCase))
                {
                    videoParams.PageSize = int.TryParse(keyValue[1], out var pageSize) ? pageSize : 10;
                }
                else if (keyValue[0].Equals("Category", StringComparison.OrdinalIgnoreCase))
                {
                    videoParams.Category = int.TryParse(keyValue[1], out var category) ? category : throw new ArgumentException("Category is required and must be an integer.");
                }
            }
        }

        // Ensure required properties are set
        if (videoParams.Category == 0)
        {
            throw new ArgumentException("Category is required and must be provided.");
        }

        return videoParams;
    }

    public static bool TryParse(string? input, IFormatProvider? formatProvider, out VideoParams result)
    {
        try
        {
            result = Parse(input ?? string.Empty, formatProvider);
            return true;
        }
        catch
        {
            result = null!;
            return false;
        }
    }
}
