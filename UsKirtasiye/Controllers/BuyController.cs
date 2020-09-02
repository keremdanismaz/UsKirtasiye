using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UsKirtasiye.DB;
using UsKirtasiye.Models;
using UsKirtasiye.Shared;

namespace UsKirtasiye.Controllers
{
    public class BuyController : Controller
    {
        private UsKirtasiyeEntities context;

        public BuyController()
        {
            context = new UsKirtasiyeEntities();
        }

        [HttpGet]
        public ActionResult Buy()
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null || Session["Basket"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            List<Models.Product_Detail.ProductBasketModels> model = (List<Models.Product_Detail.ProductBasketModels>)Session["Basket"];
            if (model == null)
            {
                model = new List<Models.Product_Detail.ProductBasketModels>();
            }
            TempData["TotalPrice"] = model.Select(x => x.Products.Price * x.Count).Sum();
            int id = currentuser.Id;
            ViewBag.adresler = context.Addresses.Where(x => x.Member_Id == id).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Buy(string Address)
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else if (Address == null)
            {
                TempData["MyError"] = "Lutfen profilinizden adres bilgilerinizi giriniz";
            }
            else
            {
                try
                {
                    var basket = (List<Models.Product_Detail.ProductBasketModels>)Session["Basket"];
                    var adresçevirme = Int64.Parse(Address);
                    var adress = context.Addresses.FirstOrDefault(x => x.Id == adresçevirme);
                    // Ödeme bildilrimi yapıldı
                    // Spariş verildi
                    // Ödeme Onaylandı
                    var order = new DB.Orders()
                    {
                        AddedDate = DateTime.Now,
                        Address = adress.AdresDescription,
                        Member_Id = currentuser.Id,
                        Id = Guid.NewGuid(),
                        Status = "SV",
                    };
                    foreach (Models.Product_Detail.ProductBasketModels item in basket)
                    {
                        var orderdetail = new DB.OrderDetails();
                        orderdetail.AddedDate = DateTime.Now;
                        orderdetail.Price = item.Products.Price * item.Count;
                        orderdetail.Product_Id = item.Products.Id;
                        orderdetail.Quantity = item.Count;
                        orderdetail.Id = Guid.NewGuid();
                        order.OrderDetails.Add(orderdetail);
                        var product = context.Products.FirstOrDefault(x => x.Id == item.Products.Id);
                        if (product != null && product.UnitsInStock >= item.Count)
                        {
                            product.UnitsInStock = product.UnitsInStock - item.Count;
                        }
                        else
                        {
                            throw new Exception(string.Format("{0} ürünü için yeterli stok yoktur veya silinmiş bir ürünü almaya çalışıyorsunuz", item.Products.Name));
                        }
                    }
                    context.Orders.Add(order);
                    context.SaveChanges();
                    await SendMail(currentuser, basket, order);
                }
                catch (Exception ex)
                {
                    ViewBag.MyError = ex.Message;
                    throw;
                }
            }

            return RedirectToAction("Buy", "Buy");
        }

        private static async Task SendMail(Members currentuser, List<Models.Product_Detail.ProductBasketModels> basket, Orders order)
        {
            string templatePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate/template.html");
            string html = System.IO.File.ReadAllText(templatePath);
            html = html.Replace("&&Ad&&", currentuser.Name);
            html = html.Replace("&&Soyad&&", currentuser.Surname);
            html = html.Replace("&&Eamil&&", currentuser.Email);
            html = html.Replace("&&Tel&&", currentuser.PhoneNumber);
            html = html.Replace("&&Adres&&", order.Address);
            string products = "";
            foreach (Models.Product_Detail.ProductBasketModels item in basket)
            {
                products += $@"<tr> <td>
                                                                            Ürün Adı:
                                                                        </td>
                                                                        <td>{item.Products.Name}</td>
                                                                    </tr>
                                                                    <tr>

                                                                        <td>
                                                                            Ürün Fiyatı:
                                                                        </td>
                                                                        <td>{item.Products.Price}</td>
                                                                    </tr>
                                                                    <tr>

                                                                        <td>
                                                                            Ürün Adet Bilgisi:
                                                                        </td>
                                                                        <td>{item.Count}</td>
                                                                    </tr>";
            }
            html = html.Replace("&&Urun&&", products);
            var emailModel = new EmailModel()
            {
                FromEmailAdress = "danismaz2000@gmail.com",
                ToEmailAdress = "danismaz2000@gmail.com",
                HtmlContent = html,
                Subject = "Yeni bir spariş alındı."
            };
            await EmailSender.SendMail(emailModel);
        }

        [HttpGet]
        public ActionResult Sparişlerim()
        {
            var currentuser = (UsKirtasiye.DB.Members)Session["LogonUser"];
            if (Session["LogonUser"] == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else
            {
                var orders = context.Orders.Where(x => x.Member_Id == currentuser.Id);
                List<Models.OrderModels> model = new List<OrderModels>();
                foreach (var item in orders)
                {
                    var bymodel = new OrderModels();
                    bymodel.TotalPrice = item.OrderDetails.Sum(y => y.Price);
                    bymodel.OrderName = string.Join(", ", item.OrderDetails.Select(y => y.Products.Name + "(" + y.Quantity + ")"));
                    model.Add(bymodel);
                }

                return View(model);
            }
        }
    }
}