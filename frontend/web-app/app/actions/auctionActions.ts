'use server'
import { Auction, PagedResult } from "@/types";
import { getTokenWorkaround } from "./authActions";
import { fetchWrapper } from "@/lib/fetchwrapper";
import {FieldValues} from "react-hook-form";
import {revalidatePath} from "next/cache";

export async function getData(query:string): Promise<PagedResult<Auction>> {

    const res = await fetchWrapper.get(`search${query}`);
    return res;
 
}

export async function updateAuctionTest(){

 
    const data = { 
        milage:Math.floor(Math.random()*10000) + 1
    }

   // const token = await getTokenWorkaround();
    return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c',data);
  
}

export async function createAuction(data:FieldValues)   {
    return await fetchWrapper.post("auctions",data);
}

export async function updateAuction(id:string,data:FieldValues){
    const res =  await fetchWrapper.put(`auctions/${id}`,data);
    revalidatePath(`/auctions/${id}`);
    return res;
}

export async function getDetailedViewData(id:string):Promise<Auction>{
    return await fetchWrapper.get(`auctions/${id}`);
   
}

export async function deleteAuction(id:string){
    const res = fetchWrapper.del(`auctions/${id}`);
    revalidatePath(`/auctions`);
    return res;
}