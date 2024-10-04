﻿using MimeKit;
using System.Data;
using MailKit.Net.Smtp;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MailKit.Security;
using MimeKit;
using MimeKit.Cryptography;
using MailKit;
using DTOs.Post;
using Services.Interfaces;
using Data_Access.Interfaces;
using Domain_Models;

namespace Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly INewsLetterRepository _newsletterRepository;
        public EmailService(IConfiguration config, INewsLetterRepository repo) 
        {
            _config = config;
            _newsletterRepository = repo;
        }
        public void SendEmail(PostCreateDto createdPost, string authorFullName)
        {
            var filteredEmails = _newsletterRepository.FilterEmailsByAuthorAndTags(authorFullName, createdPost.Tags);

            int port = int.Parse(_config["EmailPort"]);
            InternetAddressList emailList = new InternetAddressList();

            foreach (NewsLetter oneMail in filteredEmails)
            {
                emailList.Add(MailboxAddress.Parse(oneMail.Email));
            }

            //var request = new EmailObj() { };
            //var userDb = await _userRepository.GetUserByEmail(dBemail);
            //if (userDb == null)
            //{
            //    throw new DataException($"There is no user with this email {dBemail}.");
            //}
            string input = String.Format($"The author {authorFullName} created a post,\n the title of the post is {createdPost.Title} containing the tags\n{createdPost.Tags}");

            //string mailstring = "Blah blah blah blah. Click <a href=\"http://127.0.0.1:5500/src/index.html\">here</a> for more information.";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.AddRange(emailList);
            email.Subject = "New post notification";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = input };

            

            using var smpt = new MailKit.Net.Smtp.SmtpClient(); // mailkit.net.smpt

            smpt.Connect(_config["EmailHost"], port , SecureSocketOptions.SslOnConnect);
            smpt.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
            smpt.Send(email);
            smpt.Disconnect(true);
        }
    }
}
