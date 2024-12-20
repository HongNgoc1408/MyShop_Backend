﻿

using System.IO;

namespace MyShop_Backend.Storages
{
	public class FileStorage : IFileStorage
	{
		public void Delete(string path)
		{
			// Kết hợp đường dẫn hiện tại với đường dẫn tương đối để có đường dẫn tuyệt đối
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		public void Delete(IEnumerable<string> paths)
		{
			foreach (var path in paths)
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}
		}

		public async Task<string> GetBase64Async(string path)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);
			if (File.Exists(filePath))
			{
				// Chuyển đổi mảng byte thành chuỗi Base64 và trả về  
				// Đọc tất cả byte từ tệp tin một cách bất đồng bộ
				return Convert.ToBase64String(
					await File.ReadAllBytesAsync(filePath)
					);
			}
			else
			{
				return string.Empty;
			}
		}

		public async Task<IEnumerable<string>> GetBase64Async(IEnumerable<string> paths)
		{
			var tasks = paths.Select(async path =>
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);
				if (File.Exists(filePath))
				{
					var fileBytes = await File.ReadAllBytesAsync(filePath);
					return Convert.ToBase64String(fileBytes);
				}
				return string.Empty;
			});

			var results = await Task.WhenAll(tasks);
			return results.Where(result => result != string.Empty);
		}

		public async Task SaveAsync(string path, IFormFile file, string fileName)
		{
			var p = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			if (!Directory.Exists(p))
			{
				Directory.CreateDirectory(p);
			}
			var filePath = Path.Combine(p, fileName);

			using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
				await file.CopyToAsync(stream);
			}
		}

		public Task SaveAsync(string path, IFormFileCollection files, IList<string> fileNames)
		{
			var p = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			if (!Directory.Exists(p))
			{
				Directory.CreateDirectory(p);
			}
			var task = files.Select(async (file, index) =>
			{
				var filePath = Path.Combine(p, fileNames[index]);
				using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
				{
					await file.CopyToAsync(stream);
				}
			});
			return Task.WhenAll(task);
		}

		public async Task SaveAsync(string path, IEnumerable<IFormFile> files, IList<string> fileNames)
		{
			var p = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			if (!Directory.Exists(p))
			{
				Directory.CreateDirectory(p);
			}

			var tasks = files.Select(async (file, index) =>
			{
				var filePath = Path.Combine(p, fileNames[index]);
				using var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
				await file.CopyToAsync(stream);
			});

			await Task.WhenAll(tasks);
		}
	}
}
