﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;

namespace TeamplateHotel.Controllers
{
    public class BookingController : Controller
    {
        [HttpGet]
        public ActionResult MakeReservation()
        {
            using (var db = new MyDbDataContext())
            {
                var bookRoom = new BookRoom();
                bookRoom.CheckIn = DateTime.Now;
                bookRoom.CheckOut = DateTime.Now.AddDays(1);
                bookRoom.Adult = 1;
                bookRoom.Child = 0;
                if (!string.IsNullOrEmpty(Request.Params["Checkin"]))
                {
                    try
                    {
                        DateTime checkIn = Convert.ToDateTime(Request.Params["Checkin"]);
                        bookRoom.CheckIn = checkIn;
                    }
                    catch (Exception)
                    {
                        bookRoom.CheckIn = DateTime.Now;
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(Request.Params["Checkout"]))
                {
                    try
                    {
                        DateTime checkOut = Convert.ToDateTime(Request.Params["Checkout"]);
                        bookRoom.CheckOut = checkOut;
                    }
                    catch (Exception)
                    {
                        bookRoom.CheckOut = DateTime.Now.AddDays(1);
                        throw;
                    }
                }
                if (bookRoom.CheckOut <= bookRoom.CheckIn)
                {
                    bookRoom.CheckOut = bookRoom.CheckIn.AddDays(1);
                }

                if (!string.IsNullOrEmpty(Request.Params["Adult"]))
                {
                    try
                    {
                        int adult = 1;
                        int.TryParse(Request.Params["Adult"], out adult);
                        bookRoom.Adult = adult;
                    }
                    catch (Exception)
                    {
                        bookRoom.Adult = 1;
                        throw;
                    }
                }

                if (!string.IsNullOrEmpty(Request.Params["Child"]))
                {
                    try
                    {
                        int child = 0;
                        int.TryParse(Request.Params["Child"], out child);
                        bookRoom.Child = child;
                    }
                    catch (Exception)
                    {
                        bookRoom.Child = 0;
                        throw;
                    }
                }
                List<ListRoomBooking> listRoomBookings =
                    db.Rooms.Where(a => a.Status && a.LanguageID == Request.Cookies["LanguageID"].Value).Select(a => new ListRoomBooking
                    {
                        RoomId = a.ID,
                        NameRoom = a.Title,
                        Price = (double) a.Price,
                        MaxPeople = a.MaxPeople,
                        Number = 0,
                        Content = a.Content,
                    }).ToList();
                var optionAdult = new List<SelectListItem>();
                for (int i = 1; i < 11; i++)
                {
                    optionAdult.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }
                ViewBag.OptionAdult = optionAdult;

                var optionChild = new List<SelectListItem>();
                for (int i = 0; i < 11; i++)
                {
                    optionChild.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }
                ViewBag.OptionChild = optionChild;
                bookRoom.ListRoomBookings = listRoomBookings;
                return View(bookRoom);
            }
        }

        [HttpPost]
        public ActionResult SendBooking(BookRoom model)
        {
            string status = "success";
         
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Hotel hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
                        string codeBooking = hotel.CodeBooking + "1";
                        if (db.BookRooms.Any())
                        {
                            codeBooking = hotel.CodeBooking +
                                          (db.BookRooms.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1);
                        }
                        model.Code = codeBooking;
                        string infoBooking = "";
                        decimal totelPrice = 0;
                        foreach (ListRoomBooking item in model.ListRoomBookings)
                        {
                            if (item.Number > 0)
                            {
                                infoBooking += item.NameRoom + " = " + item.Number + ", ";
                                totelPrice += (decimal)item.Price*item.Number;
                            }
                        }
                        model.TotalMoney = totelPrice;
                        model.DateBook = DateTime.Now;
                        model.InfoBooking = infoBooking;
                        db.BookRooms.InsertOnSubmit(model);
                        db.SubmitChanges();
                        //Gửi email xác nhận đặt phòng
                        SendEmail sendEmail =
                        db.SendEmails.FirstOrDefault(
                            a => a.Type == TypeSendEmail.BookRoom && a.LanguageID == Request.Cookies["LanguageID"].Value);

                        sendEmail.Title = sendEmail.Title.Replace("{HotelName}", hotel.Name);
                        string content = sendEmail.Content;
                        content = content.Replace("{Code}", model.Code);
                        content = content.Replace("{Gender}", model.Gender);
                        content = content.Replace("{FullName}", model.FullName);
                        content = content.Replace("{Email}", model.Email);
                        content = content.Replace("{Tel}", model.Phone);
                        content = content.Replace("{Address}", model.Address);
                        content = content.Replace("{City}", model.City);
                        content = content.Replace("{Country}", model.Country);
                        content = content.Replace("{Smoking}", model.Smoking ? "Yes" : "No");
                        content = content.Replace("{ArrivalFlight}", model.ArrivalFlight);
                        content = content.Replace("{ArrivalTime}", model.ArrivalTime);
                        content = content.Replace("{Request}", model.Request);
                        content = content.Replace("{InfoBooking}", model.InfoBooking);
                        content = content.Replace("{CheckIn}", model.CheckIn.ToString("MMMM, dd, yyyy"));
                        content = content.Replace("{CheckOut}", model.CheckOut.ToString("MMMM, dd, yyyy"));
                        content = content.Replace("{Adult}", model.Adult.ToString());
                        content = content.Replace("{Child}", model.Child.ToString());
                        content = content.Replace("{TotalPrice}", model.TotalMoney.ToString("N"));
                        content = content.Replace("{HotelName}", hotel.Name);
                        content = content.Replace("{HotelEmail}", hotel.Email);
                        content = content.Replace("{HotelTel}", hotel.Tel);
                        content = content.Replace("{Website}", hotel.Website);

                        //gửi email cho khách hàng
                        MailHelper.SendMail(hotel.Email, model.Email, sendEmail.Title, content);
                        //gửi email cho khách sạn
                        MailHelper.SendMail(model.Email, hotel.Email, hotel.Name + " (" + model.Code+")- Booking room of " + model.FullName, content);
                        
                    }
                }
                catch(Exception ex)
                {
                    status = "error";
                }
         
            return Redirect("/Booking/Messages/?status=" + status);
        }

        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.BookRoom && a.LanguageID == Request.Cookies["LanguageID"].Value);

                string status = Request.Params["status"];
                ViewBag.Messages = "";
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status.Equals("success"))
                    {
                        ViewBag.Messages = sendEmail.Success;
                    }
                    else
                    {
                        ViewBag.Messages = sendEmail.Error;
                    }
                }
                else
                {
                    ViewBag.Messages = sendEmail.Error;
                }
                return View();
            }
        }
    }
}