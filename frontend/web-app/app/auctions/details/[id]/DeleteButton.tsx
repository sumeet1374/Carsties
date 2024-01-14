'use client'
import React, {useState} from 'react';
import {Button} from "flowbite-react";
import Link from "next/link";
import {useRouter} from "next/navigation";
import {deleteAuction} from "@/app/actions/auctionActions";
import {toast} from "react-hot-toast";

type Props = {
    id:string
};
export default function DeleteButton({ id}:Props){
    const router = useRouter();
    const [loading,setLoading] = useState(false);
    
    function onDelete(){
        setLoading(true);
        deleteAuction(id).then((res)=> {
           
            if(res.error) 
                throw res.error;
            router.push("/");
        })
            .catch((error)=> {
               
                toast.error(error.status + "  " + error.message);
                
            })
            .finally(()=> setLoading(false));
    }
    return <Button color="failure" isProcessing={loading} onClick={onDelete}>
        Delete Auction
    </Button>
}