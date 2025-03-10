﻿using WebStudyServer.Repo;
using WebStudyServer.Model;
using Proto;
using WebStudyServer.Helper;

namespace WebStudyServer.Manager
{
    public partial class PointManager : UserManagerBase<PointModel>
    {
        public double Amount => _model.Amount;
        public double AccAmount => _model.AccAmount;
        public PointManager(UserRepo userRepo, PointModel model) : base(userRepo, model)
        {
        }

        public double DecAmount(double amount, string reason)
        {
            var befAmount = _model.Amount;
            var befAccAmount= _model.AccAmount;

            ReqHelper.ValidEnough(amount, befAmount, $"POINT_{_model.Num}", reason);

            _model.Amount -= amount;
            _model.AccAmount -= amount;
            _userRepo.Point.UpdateMdl(_model);
            return _model.Amount;
        }

        public double IncAmount(double amount, string reason)
        {
            var befAmount = _model.Amount;
            var befAccAmount = _model.AccAmount;

            _model.Amount += amount;
            _model.AccAmount += amount;
            _userRepo.Point.UpdateMdl(_model);
            return _model.Amount;
        }

    }
}
