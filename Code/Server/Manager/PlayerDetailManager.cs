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

        public CashPacket GetCashPacket()
        {
            return new CashPacket
            {
                FreeCash = _model.FreeCash,
                RealCash = _model.RealCash,
            };
        }

        // TODO: Reward관련 내용 별도 멤버변수로 빼는것 고려
        public ChgObjPacket DecCost(ObjPacket valCostObj, string reason)
        {
            var amount = DecCost(valCostObj.Type, valCostObj.Num, valCostObj.Amount, reason);
            var obj = new ChgObjPacket
            {
                TotalAmount = amount,
                Amount = valCostObj.Amount,
                Type = valCostObj.Type,
                Num = valCostObj.Num,
            };
            return obj;
        }

        public double DecCost(EObjType objType, int objNum, double objAmount, string reason)
        {
            // 마이너스, 소수점 체크
            ReqHelper.ValidUnderFlowParam(objAmount, reason);
            var valObjAmount = ReqHelper.ValidWithoutDecimal(objAmount, reason);

            var objTypeCategory = objType.ToObjTyeCategory();
            switch (objTypeCategory)
            {
                case EObjType.EXP:
                    var exp = DecExpInternal(valObjAmount, reason);
                    return exp;
                case EObjType.GOLD:
                    var gold = DecGoldInternal(valObjAmount, reason);
                    return gold;
                case EObjType.TOTAL_CASH:
                    var totalCash = DecCashInternal(valObjAmount, reason);
                    return totalCash;
                case EObjType.POINT_START:
                    var pointNum = (int)objType;
                    var pointAmount = DecPointInternal(pointNum, valObjAmount, reason);
                    return pointAmount;
                case EObjType.TICKET_START:
                    var ticketNum = (int)objType;
                    var ticketAmount = DecTicketInternal(ticketNum, valObjAmount, reason);
                    return ticketAmount;
                case EObjType.ITEM:
                    var itemAmount = DecItemInternal(objNum, valObjAmount, reason);
                    return itemAmount;
                default:
                    throw new GameException(EErrorCode.PARAM, "NO_HANDLING_COST_OBJ_TYPE", new { ObjType = objType });
            }
        }

        public List<ChgObjPacket> IncRewardList(List<ObjValue> valRewardObjValList, string reason)
        {
            var objList = new List<ChgObjPacket>();
            foreach (var valReward in valRewardObjValList)
            {
                var obj = IncReward(valReward, reason);
                objList.Add(obj);
            }
            return objList;
        }


        public List<ChgObjPacket> IncRewardList(List<ObjPacket> valRewardListObj, string reason)
        {
            var objList = new List<ChgObjPacket>();
            foreach(var valReward in valRewardListObj)
            {
                var obj = IncReward(valReward, reason);
                objList.Add(obj);
            }
            return objList;
        }

        public ChgObjPacket IncReward(ObjValue valRewardObjVal, string reason)
        {
            var amount = IncReward(valRewardObjVal.Key.Type, valRewardObjVal.Key.Num, valRewardObjVal.Value, reason);
            var obj = new ChgObjPacket
            {
                TotalAmount = amount,
                Amount = valRewardObjVal.Value,
                Type = valRewardObjVal.Key.Type,
                Num = valRewardObjVal.Key.Num,
            };
            return obj;
        }

        public ChgObjPacket IncReward(ObjPacket valRewardObj, string reason)
        {
            var amount = IncReward(valRewardObj.Type, valRewardObj.Num, valRewardObj.Amount, reason);
            var obj = new ChgObjPacket
            {
                TotalAmount = amount,
                Amount = valRewardObj.Amount,
                Type = valRewardObj.Type,
                Num = valRewardObj.Num,
            };
            return obj;
        }

        public double IncReward(EObjType objType, int objNum, double objAmount, string reason)
        {
            // 마이너스, 소수점 체크
            ReqHelper.ValidUnderFlowParam(objAmount, reason);
            var valObjAmount = ReqHelper.ValidWithoutDecimal(objAmount, reason);

            var objTypeCategory = objType.ToObjTyeCategory();
            switch (objTypeCategory)
            {
                case EObjType.GOLD:
                    var gold = IncGoldInternal(valObjAmount, reason);
                    return gold;
                case EObjType.EXP:
                    var exp = IncExpInternal(valObjAmount, reason);
                    return exp;
                case EObjType.REAL_CASH:
                    var realCash = IncRealCashInternal(valObjAmount, reason);
                    return realCash;
                case EObjType.FREE_CASH:
                    var freeCash = IncFreeCashInternal(valObjAmount, reason);
                    return freeCash;
                case EObjType.POINT_START:
                    var pointNum = (int)objType;
                    var pointAmount = IncPointInternal(pointNum, valObjAmount, reason);
                    return pointAmount;
                case EObjType.TICKET_START:
                    var ticketNum = (int)objType;
                    var ticketAmount = IncTicketInternal(ticketNum, valObjAmount, reason);
                    return ticketAmount;
                case EObjType.ITEM:
                    var itemAmount = IncItemInternal(objNum, valObjAmount, reason);
                    return itemAmount;
                case EObjType.COOKIE:
                    var cookieSoulStone1 = IncCookieInternal(objNum, (int)valObjAmount, reason);
                    return cookieSoulStone1;
                case EObjType.SOUL_STONE:
                    var cookieSoulStone2 = IncSoulStoneInternal(objNum, (int)valObjAmount, reason);
                    return cookieSoulStone2;
                default:
                    throw new GameException(EErrorCode.PARAM, "NO_HANDLING_REWARD_OBJ_TYPE", new { ObjType = objType });
            }
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
            _userRepo.PlayerDetail.UpdateMdl(_model);
            return _model.Gold;
        }

        private double IncGoldInternal(double amount, string reason)
        {
            var befGold = _model.Gold;
            var befAccGold = _model.AccGold;

            _model.Gold += amount;
            _model.AccGold += amount;
            _userRepo.PlayerDetail.UpdateMdl(_model);
            return _model.Gold;
        }
        #endregion

        #region EXP
        public double DecExp(double amount, string reason) => DecExpInternal(amount, reason);
        public double IncExp(double amount, string reason) => IncExpInternal(amount, reason);
        private double DecExpInternal(double amount, string reason)
        {
            var befExp = _model.Exp;
            var befAccExp = _model.AccExp;

            ReqHelper.ValidEnough(amount, befExp, "PLAYER_EXP", reason);

            _model.Exp -= amount;
            _model.AccExp -= amount;
            _userRepo.PlayerDetail.UpdateMdl(_model);
            return _model.Exp;
        }

        private double IncExpInternal(double amount, string reason)
        {
            var befExp = _model.Exp;
            var befAccExp = _model.AccExp;

            _model.Exp += amount;
            _model.AccExp += amount;
            _userRepo.PlayerDetail.UpdateMdl(_model);
            return _model.Exp;
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

            ReqHelper.ValidEnough(amount, befTotalCash, "PLAYER_TOTAL_CASH", reason);

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

            _userRepo.PlayerDetail.UpdateMdl(_model);

            var totalCash = _model.RealCash + _model.FreeCash;
            return totalCash;
        }

        private double IncFreeCashInternal(double amount, string reason)
        {
            var befGold = _model.FreeCash;
            var befAccGold = _model.AccFreeCash;

            _model.FreeCash += amount;
            _model.AccFreeCash += amount;
            _userRepo.PlayerDetail.UpdateMdl(_model);
            return _model.FreeCash;
        }

        private double IncRealCashInternal(double amount, string reason)
        {
            var befGold = _model.RealCash;
            var befAccGold = _model.AccRealCash;

            _model.RealCash += amount;
            _model.AccRealCash += amount;
            _userRepo.PlayerDetail.UpdateMdl(_model);
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
        private double IncCookieInternal(int cookieNum, int amount, string reason)
        {
            var mgrCookie = _userRepo.Cookie.Touch(cookieNum);
            var soulStone = mgrCookie.IncCookie(amount, reason);
            return soulStone;
        }

        private double IncSoulStoneInternal(int soulStoneNum, int amount, string reason)
        {
            var mgrCookie = _userRepo.Cookie.TouchBySoulStone(soulStoneNum);
            var soulStone = mgrCookie.IncSoulStone(amount, reason);
            return soulStone;
        }
        #endregion

        #region ITEM
        private double DecItemInternal(int itemNum, double amount, string reason)
        {
            var mgrItem = _userRepo.Item.Touch(itemNum);
            var itemAmount = mgrItem.DecAmount(amount, reason);
            return itemAmount;
        }

        private double IncItemInternal(int itemNum, double amount, string reason)
        {
            var mgrItem = _userRepo.Item.Touch(itemNum);
            var itemAmount = mgrItem.IncAmount(amount, reason);
            return itemAmount;
        }
        #endregion


    }
}
