﻿using Proto;
using WebStudyServer.Base;
using WebStudyServer.Helper;
using WebStudyServer.Manager;
using WebStudyServer.Model;
using WebStudyServer.Repo;

namespace WebStudyServer.Component
{
    public class SessionComponent : AuthComponentBase
    {
        public static SessionComponent CreateInstance(AuthRepo authRepo)
        {
            return new SessionComponent(authRepo);
        }

        public SessionComponent(AuthRepo authRepo) : base(authRepo)
        {
        }

        public bool TryGet(ulong accountId, out SessionManager mgrSession)
        {
            mgrSession = null;

            var repoSession = _authRepo.GetSessionByAccountId(accountId);
            if (repoSession == null)
                return false;

            mgrSession = new SessionManager(_authRepo, repoSession);
            return true;
        }

        public bool TryGetByKey(string key, out SessionManager mgrSession)
        {
            mgrSession = null;

            var repoSession = _authRepo.GetSession(key);
            if (repoSession == null)
                return false;

            mgrSession = new SessionManager(_authRepo, repoSession);
            return true;
        }

        public SessionManager Touch(ulong accountId)
        {
            var repoSession = _authRepo.GetSessionByAccountId(accountId);
            if (repoSession == null)
            {
                var newSession = new SessionModel
                {
                    Key = IdHelper.GenerateGuidKey(),
                    AccountId = accountId,
                    PublicIp = _authRepo.RpcContext.Ip,
                    ShardId = _authRepo.RpcContext.ShardId,
                    State = ESessionState.NONE,
                    ClientSecret = "",
                    EncryptSecret = "",
                    EncryptIV = "",
                    ExpireTimestamp = 0,
                    PlayerId = 0,
                    DeviceKey = "",
                };

                repoSession = _authRepo.CreateSession(newSession);
            }
            var mgrSession = new SessionManager(_authRepo, repoSession);
            return mgrSession;
        }
    }
}
