﻿using WebStudyServer.Model;
using WebStudyServer.Model;
using WebStudyServer.Repo;

namespace WebStudyServer.Manager
{
    public class AccountManager : AuthManagerBase<AccountModel>
    {
        public ulong Id => Model.Id;

        public AccountManager(AuthRepo authRepo, AccountModel model) : base(authRepo, model)
        {
        }


    }
}
