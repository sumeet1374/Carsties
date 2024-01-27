import {Bid} from "@/types";
import {create} from "zustand";

type State = {
    bids:Bid[],
    open:boolean
};

type Action = {
    setBids:(bids:Bid[])=>void;
    addBid:(bid:Bid)=>void;
    setOpen:(value:boolean)=>void;
};

export const useBidStore = create<Action & State>(
    (set)=> (
        {
            bids:[],
            open:true,
            setBids:(bids)=> {
                set(()=> (
                    {
                        bids 
                    }
                ));
            },
            addBid:(bid)=> {
                set((state)=> (
                    {
                        bids: !state.bids.find(x=>x.id === bid.id)? [bid,...state.bids]:[...state.bids]
                    }
                ));
            },
            setOpen:(value:boolean)=>{
               set(()=>{
                   return { open:value};
               });
            }
        }
    )
)