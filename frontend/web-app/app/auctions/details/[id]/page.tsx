import React from 'react'
import {deleteAuction, getBidsForAuction, getDetailedViewData} from "@/app/actions/auctionActions";
import Heading from "@/app/components/Heading";
import CountdownTimer from "@/app/auctions/CountdownTimer";
import CarImage from "@/app/auctions/CarImage";
import DetailedSpecs from "@/app/auctions/details/DetailsSpecs";
import {getCurrentUser} from "@/app/actions/authActions";
import EditButton from "@/app/auctions/details/[id]/EditButton";
import {Button} from "flowbite-react";
import DeleteButton from "@/app/auctions/details/[id]/DeleteButton";
import {param} from "ts-interface-checker";
import BidItem from "@/app/auctions/details/[id]/BidItem";
import BidList from "@/app/auctions/details/[id]/BidList";

export default async function Details({ params}:{ params:{ id:string}}) {
  
  const data = await getDetailedViewData(params.id);
  const user = await getCurrentUser();

  
  return (
      <div>
          <div className="flex justify-between">
              <div className="flex items-center gap-3">
                  <Heading title={`${data.make} ${data.model}`}></Heading>
                  {user?.username == data.seller && (
                      <>
                      <EditButton id={data.id}/>
                          <DeleteButton id={data.id}/>
                      </>
                  )}
              </div>
             
              <div className="flex gap-3">
                  <h3 className="text-2xl font-semibold">Time Remaining:</h3>

                  <CountdownTimer auctionEnd={data.auctionEnd}/>
              </div>
          </div>
          <div className="grid grid-cols-2 gap-6 mt-3">
              <div className='w-full bg-gray-200 
            aspect-w-16 
            aspect-h-10 
            rounded-lg
            overflow-hidden'
              >
                  <CarImage imageUrl={data.imageUrl}></CarImage>

              </div>
              <BidList user={user} auction={data} />

          </div>
          <div className='mt-3 grid grid-cols-1 rounded-lg'>
              <DetailedSpecs auction={data}/>
          </div>
      </div>


  )
}
