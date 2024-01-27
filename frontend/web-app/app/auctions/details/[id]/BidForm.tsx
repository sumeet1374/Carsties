'use client'
import React from "react";
import {FieldValues, useForm} from "react-hook-form";
import {useBidStore} from "@/hooks/useBidStore";
import {placeBidForAuction} from "@/app/actions/auctionActions";
import {Bid} from "@/types";
import {numberWithCommas} from "@/lib/numberwithComma";
import {toast} from "react-hot-toast";

type Props = {
    auctionId:string,
    highBid:number
}
export default function BidForm({auctionId,highBid}:Props){
    
    const {register,
        handleSubmit,
        reset,
       formState:{ errors}} = useForm();
    
    const addBid = useBidStore(state => state.addBid);
    
    function onSubmit(data:FieldValues){
        
        if(data.amount <= highBid){
            reset();
            return toast.error(`Bid must be at least $${numberWithCommas(highBid+1)}`);
        }
        placeBidForAuction(auctionId,+data.amount)
            .then((bid:any)=> {
           
                if(bid.error){
                    throw bid.error;
                   
                }
                else{
                    addBid(bid);
                }
                    
             
                reset();
            })
            .catch((error)=> {
                toast.error(error.message);
            })
    }
    return <form onSubmit={handleSubmit(onSubmit)} className='flex items-center border-2 rounded-lg py-2 '>
            <input type="number"
                   {...register("amount")}
                    className="input-custom text-sm text-gray-600"
                   placeholder={`Enter your bid (minimum bid is $${numberWithCommas(highBid + 1)}`}
            />
    </form>
}