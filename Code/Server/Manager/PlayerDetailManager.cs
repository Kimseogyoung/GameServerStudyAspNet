﻿using WebStudyServer.Repo;
using WebStudyServer.Model;
using Proto;
using WebStudyServer.Extension;
using WebStudyServer.Helper;
using Protocol;

namespace WebStudyServer.Manager
{
    public partial class PlayerDetailManager : UserManagerBase<PlayerDetailModel>
    {
        public ulong Id => Model.PlayerId;

        public PlayerDetailManager(UserRepo userRepo, PlayerDetailModel model/*, PointComponent*/) : base(userRepo, model)
        {
        }
    
        // TODO: Reward관련 내용 별도 멤버변수로 빼는것 고려
        public ObjPacket DecCost(CostObjPacket valCostObj, string reason)
        {
            var amount = DecCost(valCostObj.Type, valCostObj.Num, valCostObj.Amount, reason);
            var obj = new ObjPacket
            {
                Amount = amount,
                ChgAmount = valCostObj.Amount,
                Type = valCostObj.Type,
                Num = valCostObj.Num,
            };
            return obj;
        }

        public double DecCost(EObjType objType, int objNum, double objAmount, string reason)
        {
            // TODO: 마이너스, 소수점 체크
            var objTypeCategory = objType.ToObjTyeCategory();
            switch (objTypeCategory)
            {
                // TODO: 보유량 체크
                case EObjType.GOLD:
                    var gold = DecGoldInternal(objAmount, reason);
                    return gold;
                case EObjType.STAR_CANDY:
                    break;
                case EObjType.TOTAL_CASH:
                    var totalCash = DecCashInternal(objAmount, reason);
                    return totalCash;
                case EObjType.POINT_START:
                    var pointNum = (int)objType;
                    var pointAmount = DecPointInternal(pointNum, objAmount, reason);
                    return pointAmount;
                case EObjType.TICKET_START:
                    var ticketNum = (int)objType;
                    var ticketAmount = DecTicketInternal(ticketNum, objAmount, reason);
                    return ticketAmount;
                default:
                    throw new GameException(EErrorCode.PARAM, "NO_HANDLING_COST_OBJ_TYPE", new { ObjType = objType });
            }

            return 0;
        }

        public List<ObjPacket> IncReward(List<RewardObjPacket> valRewardListObj, string reason)
        {
            var objList = new List<ObjPacket>();
            foreach(var valReward in valRewardListObj)
            {
                var obj = IncReward(valReward, reason);
                objList.Add(obj);
            }
            return objList;
        }

        public ObjPacket IncReward(RewardObjPacket valRewardObj, string reason)
        {
            var amount = DecCost(valRewardObj.Type, valRewardObj.Num, valRewardObj.Amount, reason);
            var obj = new ObjPacket
            {
                Amount = amount,
                ChgAmount = valRewardObj.Amount,
                Type = valRewardObj.Type,
                Num = valRewardObj.Num,
            };
            return obj;
        }

        public double IncReward(EObjType objType, int objNum, double objAmount, string reason)
        {
            // TODO: 마이너스, 소수점 체크
            var objTypeCategory = objType.ToObjTyeCategory();
            switch (objTypeCategory)
            {
                case EObjType.GOLD:
                    var gold = IncGoldInternal(objAmount, reason);
                    return gold;
                case EObjType.STAR_CANDY:
                    break;
                case EObjType.REAL_CASH:
                    var realCash = IncRealCashInternal(objAmount, reason);
                    return realCash;
                case EObjType.FREE_CASH:
                    var freeCash = IncFreeCashInternal(objAmount, reason);
                    return freeCash;
                case EObjType.POINT_START:
                    var pointNum = (int)objType;
                    var pointAmount = IncPointInternal(pointNum, objAmount, reason);
                    return pointAmount;
                case EObjType.TICKET_START:
                    var ticketNum = (int)objType;
                    var ticketAmount = IncTicketInternal(ticketNum, objAmount, reason);
                    return ticketAmount;
                case EObjType.ITEM:
                    break;
                case EObjType.COOKIE:
                    var cookieStarExp = IncCookieInternal(objNum, (int)objAmount, reason);
                    return cookieStarExp;
                case EObjType.KINGDOM_ITEM:
                    break;
                default:
                    throw new GameException(EErrorCode.PARAM, "NO_HANDLING_REWARD_OBJ_TYPE", new { ObjType = objType });
            }

            return 0;
        }

        #region GOLD
        public double DecGold(double amount, string reason) => DecGoldInternal(amount, reason);
        public double IncGold(double amount, string reason) => IncGoldInternal(amount, reason);
        private double DecGoldInternal(double amount, string reason)
        {
            var befGold = _model.Gold;
            var befAccGold = _model.AccGold;

            _model.Gold -= amount;
            _model.AccGold -= amount;
            _userRepo.PlayerDetail.Update(_model);
            return _model.Gold;
        }

        private double IncGoldInternal(double amount, string reason)
        {
            var befGold = _model.Gold;
            var befAccGold = _model.AccGold;

            _model.Gold += amount;
            _model.AccGold += amount;
            _userRepo.PlayerDetail.Update(_model);
            return _model.Gold;
        }
        #endregion

        #region CASH
        public double DecCash(double amount, string reason) => DecCashInternal(amount, reason);
        public double IncFreeCash(double amount, string reason) => IncFreeCashInternal(amount, reason);
        public double IncRealCash(double amount, string reason) => IncRealCashInternal(amount, reason);
        private double DecCashInternal(double amount, string reason)
        {
            var befFreeCash = _model.FreeCash;
            var befAccFreeCash = _model.AccFreeCash;
            var befRealCash = _model.RealCash;
            var befAccRealCash = _model.AccRealCash;
            var befTotalCash = befFreeCash + befRealCash;
            var befAccTotalCash = befAccFreeCash + befAccRealCash;

            // RealCash 먼저 소모
            double realCashCost = Math.Min(befRealCash, amount);
            double freeCashCost = amount - realCashCost;

            if (realCashCost > 0)
            {
                _model.RealCash -= realCashCost;
                _model.AccRealCash -= realCashCost;
            }

            if (freeCashCost > 0)
            {
                _model.FreeCash -= freeCashCost;
                _model.AccFreeCash -= freeCashCost;
            }

            _userRepo.PlayerDetail.Update(_model);

            var totalCash = _model.RealCash + _model.FreeCash;
            return totalCash;
        }

        private double IncFreeCashInternal(double amount, string reason)
        {
            var befGold = _model.FreeCash;
            var befAccGold = _model.AccFreeCash;

            _model.FreeCash += amount;
            _model.AccFreeCash += amount;
            _userRepo.PlayerDetail.Update(_model);
            return _model.FreeCash;
        }

        private double IncRealCashInternal(double amount, string reason)
        {
            var befGold = _model.RealCash;
            var befAccGold = _model.AccRealCash;

            _model.RealCash += amount;
            _model.AccRealCash += amount;
            _userRepo.PlayerDetail.Update(_model);
            return _model.RealCash;
        }
        #endregion

        #region POINT
        private double DecPointInternal(int pointNum, double amount, string reason)
        {
            var mgrPoint = _userRepo.Point.Touch((EObjType)pointNum);
            var pointAmount = mgrPoint.DecAmount(amount, reason);
            return pointAmount;
        }

        private double IncPointInternal(int pointNum, double amount, string reason)
        {
            var mgrPoint = _userRepo.Point.Touch((EObjType)pointNum);
            var pointAmount = mgrPoint.IncAmount(amount, reason);
            return pointAmount;
        }
        #endregion

        #region TICKET
        private double DecTicketInternal(int ticketNum, double amount, string reason)
        {
            var mgrPoint = _userRepo.Ticket.Touch((EObjType)ticketNum);
            var pointAmount = mgrPoint.DecAmount(amount, reason);
            return pointAmount;
        }

        private double IncTicketInternal(int ticketNum, double amount, string reason)
        {
            var mgrPoint = _userRepo.Ticket.Touch((EObjType)ticketNum);
            var pointAmount = mgrPoint.IncAmount(amount, reason);
            return pointAmount;
        }
        #endregion

        #region COOKIE
        private double IncCookieInternal(int cookieNum, int starExp, string reason)
        {
            var mgrPoint = _userRepo.Cookie.Touch(cookieNum);
            var pointAmount = mgrPoint.IncStarExp(starExp, reason);
            return pointAmount;
        }
        #endregion


    }
}