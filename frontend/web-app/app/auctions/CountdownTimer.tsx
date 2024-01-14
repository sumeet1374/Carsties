'use client'

import React from 'react'
import Countdown,{zeroPad}  from 'react-countdown';

type Props = {
    auctionEnd:string
}

// Renderer callback with condition
const renderer = ({ days,hours, minutes, seconds, completed }:{days:Number,hours:Number,minutes:Number,seconds:Number,completed:boolean}) => {
    
    // @ts-ignore
    // @ts-ignore
    return (

        <div className={
            `border-2  border-white text-white py-1 px-2 rounded-lg flex justify-center
            ${completed? 'bg-red-600': (days === 0 && hours < 10)?
            'bg-amber-600':'bg-green-600'
            }
            `
        }>
           {completed ? ( <span>Auction Finished</span>):(<span suppressHydrationWarning={true}>{zeroPad(days).toString()}:{zeroPad(hours).toString()}:{zeroPad(minutes).toString()}:{zeroPad(seconds).toString()}</span>)}
        </div>
    )
    
    // if (completed) {
    //   // Render a completed state
    //   return <span>Finished</span>;
    // } else {
    //   // Render a countdown
    //   return <span>{days.toString()}:{hours.toString()}:{minutes.toString()}:{seconds.toString()}</span>;
    // }
  };
  
export default function CountdownTimer({auctionEnd}:Props) {
  return (
    <div><Countdown date={auctionEnd} renderer={renderer} /></div>
  )
}
