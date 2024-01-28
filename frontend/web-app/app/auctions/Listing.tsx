'use client'
import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '@/types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';
import {useAuctionStore} from "@/hooks/useAuctionStore";
import {useBidStore} from "@/hooks/useBidStore";

export default function Listing() {

  //  const [data, setData] = useState<PagedResult<Auction>>();
    const [loading,setLoading] = useState(true);
    const params = useParamsStore((state) => ({
        pageNumber: state.pageNumber,
        pageSize: state.pageSize,
        searchTearm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy,
        seller:state.seller,
        winner:state.winner
    }), shallow);
    const setParams = useParamsStore((state) => state.setParams);
    const url = qs.stringifyUrl({ url: '', query: params });
   const data = useAuctionStore((state)=> ({
       auctions: state.auctions,
       totalCount: state.totalCount,
       pageCount:state.pageCount
   }),shallow);
    const setData = useAuctionStore((state)=> state.setData);

    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber });
    }

    useEffect(() => {
        getData(url).then((data) => {
            
            setData(data);
            setLoading(false);

        })
    }, [url,setData]);

    if (loading)
        return <h3>Loading .....</h3>

  


    return <>
        <Filters />
        {data.totalCount === 0 ? (<EmptyFilter showReset />) : (
            <>
                <div className='grid grid-cols-4 gap-6'>
                    {data.auctions.map((auction) => (
                        <AuctionCard auction={auction} key={auction.id} />
                    ))}
                </div>
                <div className='flex justify-center mt-4'>
                    <AppPagination currentPage={params.pageNumber} pageCount={data.pageCount} pageChanged={setPageNumber} />
                </div>
            </>
        )}


    </>
}

