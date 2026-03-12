using System;
using System.Collections.Specialized;
using System.Management;
using System.Net;

namespace Tamphan_WorkingBCMBP_WF.Services
{
    public static class MachineService
    {   //cần phải tạo google form dạng khảo sát
        // lưu ý khi tạo form, cần thêm 3 trường dạng "Short answer" với tên lần lượt là "MachineID", "Customer", "Password" để lưu thông tin máy và thời gian gửi, sau đó lấy Url bằng cách vào https://docs.google.com/forms, lưu ý là phải vào source tạo form nha, không phải cái excel là https://docs.google.com/spreadsheets/ đâu, thì publish nó, mở tab RESPONSE, link nó với sheet tạo trong Google Sheet, sau đó copy link formResponse để thay thế vào biến formUrl bên dưới, lúc này link mình copy được sẽ có dạng này https://docs.google.com/forms/d/e/1FAIpQLSc1pLNSl3X34MviTLKt1SRLzN4cYxuPkLRI75WJETDsQs2naQ/viewform?usp=dialog, ta phải xóa cái đuôi /viewform?usp=dialog đi và thay bằng /formResponse, thì mới có thể gửi dữ liệu vào form được, nếu không sẽ bị lỗi 404 not found, vì Google Form chỉ chấp nhận dữ liệu gửi vào đúng endpoint formResponse mà thôi, còn viewform chỉ là trang hiển thị form cho người dùng thôi, nó không nhận dữ liệu gửi vào đâu.
        private static string formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSc1pLNSl3X34MviTLKt1SRLzN4cYxuPkLRI75WJETDsQs2naQ/formResponse";

        public static string GetMachineId()
        {
            var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["ProcessorId"]?.ToString() ?? "UNKNOWN";
            }

            return "UNKNOWN";
        }

        public static void SendMachineId(string customer, string password)
        {
                string machineId = GetMachineId();
                string computerName = Environment.MachineName;
                string windowsUser = Environment.UserName;  

            var data = new NameValueCollection
                {   
                    ["entry.1190443877"] = windowsUser,
                    ["entry.1048383523"] = computerName,
                    ["entry.824481634"] = machineId,
                    ["entry.1713931869"] = customer,
                    ["entry.1600555180"] = password
                };
            // Các entry.XXXXXX phải thay thế bằng entry tương ứng với từng trường trong form của bạn, có thể lấy được bằng cách xem source của form và tìm kiếm "entry", và nếu bạn đang ở previewmode, click vào một chỗ bất kỳ nào trong form, sau đó click chuột phải chọn "Inspect" để mở DevTools, tìm đến phần form và tìm kiếm "entry" sẽ thấy được các entry, thì sẽ có duy nhất một element có entry, nhưng nó lại bị ẩn mất các đoạn nội dung đằng sau, nó chỉ hiển thị một phần nhỏ và không có các thông tin entry bạn cần như ở dưới, nguyên nhân là Google Form thường ẩn field thật, nên khi bạn inspect HTML chỉ thấy một đoạn hidden. Do đó bạn cần phải copy đoạn HTML đó, sau đó dán vào một trình soạn thảo code nào đó như Notepad++ để xem được toàn bộ nội dung, lúc đó bạn sẽ thấy được các entry cần thiết để điền dữ liệu vào form. Nếu bạn không làm bước này thì sẽ không biết được entry nào tương ứng với trường nào trong form, và khi gửi dữ liệu sẽ không đúng định dạng mà Google Form yêu cầu, dẫn đến việc dữ liệu không được ghi nhận vào sheet.
            //Dưới đây là một cách khác để tìm entry, đó là bạn ra chế độ tạo form bình thường, không ở trong previewmode nữa, nhưng lúc này entry cũng sẽ bị ẩn, và thậm chí nó còn xuất hiện rất nhiều entry ở các chỗ khác nhau làm bạn không xác định được, nên còn fail hơn cách trên, và nút submit cũng sẽ không có khả dụng luôn. Như vậy lúc này bạn xử lý như sau: bạn nhấn lên Published ở trên tab, nó sẽ bật ra các Published Option, và bạn hãy click và "Copy responder link", một link dạng "https://docs.google.com/forms/d/e/1FAIpQLSc1pLNSl3X34MviTLKt1SRLzN4cYxuPkLRI75WJETDsQs2naQ/viewform?usp=dialog" sẽ hiện ra, bạn bấm copy và dán nó qua tab kế bên trên trình duyệt, rồi enter, nó sẽ ra link người dùng thật dùng để submit, nút submit đã khả dụng, và bây giờ bạn hãy rightclick xong chon "Inspect" để mở DevTools, chọn qua tab Network, xong bạn trở lại form, nhập các giá trị cho form, nhấn submit, sau khi submit thành công, bạn quay lại Network, nhìn xuống dưới, sẽ có một Name là formResponse nằm ngay ở đầu tiên của Network, bạn click vào nó, mở qua tab Payload, nó sẽ hiện các entry tương ứng, và nó trả về cùng 1 kết quả với cách ở trên.

            using (WebClient wc = new WebClient())
                {
                    wc.UploadValues(formUrl, "POST", data);
                }
        }
    }
}