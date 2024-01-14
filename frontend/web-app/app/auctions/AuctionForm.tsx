'use client'

import {Button, TextInput} from 'flowbite-react';
import React, {useEffect} from 'react'
import {FieldValues, useForm} from 'react-hook-form';
import Input from '../components/Input';
import DateInput from '../components/Dateinput';
import {createAuction, updateAuction} from '../actions/auctionActions';
import {usePathname, useRouter} from "next/navigation";
import {toast} from "react-hot-toast";
import {Auction} from "@/types";

type Props = {
    auction?: Auction
}
export default function AuctionForm({auction}: Props) {

    const {
        register,
        control,
        handleSubmit,
        setFocus, reset,
        formState: {isSubmitting, isValid, isDirty, errors},
    } = useForm({mode: 'onTouched'});

    const router = useRouter();
    const pathName = usePathname();


    async function onSubmit(data: FieldValues) {
        console.log(data);
        try {
            let id = "";
            let result;
            if (pathName === "/auctions/create") {
                result = await createAuction(data);
                id = result.id;

            } else {
                if (auction) {
                    result = await updateAuction(auction.id, data);
                    id = auction.id;
                }
            }

            if (result.error) {
                throw result.error;
            }
            router.push(`/auctions/details/${id}`);

        } catch (error: any) {
            console.log(error);
            toast.error(error.status + " " + error.message);
        }

    }

    useEffect(() => {
        if (auction) {
            const {make, model, color, mileage, year} = auction;
            reset({make, model, color, mileage, year});
        }
        setFocus('make')
    }, [setFocus]);

    return (
        <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>

            <Input name="make" control={control} label='Make' rules={{required: 'Make is required'}}/>
            <Input name="model" control={control} label='Model' rules={{required: 'Model is required'}}/>
            <Input name="color" control={control} label='Color' rules={{required: 'Color is required'}}/>
            <div className='grid grid-cols-2 gap-3'>
                <Input name="year" control={control} label='Year' type='number' rules={{required: 'Year is required'}}/>
                <Input name="mileage" control={control} label='Mileage' type='number'
                       rules={{required: 'Mileage is required'}}/>
            </div>
            { pathName === "/auctions/create"  &&
            <>
                <Input name="imageUrl" control={control} label='Image URL' rules={{required: 'Image URL is required'}}/>
                <div className='grid grid-cols-2 gap-3'>
                    <Input name="reservePrice" control={control} label='Reserve price (enter 0 if no reserve)'
                           type='number' rules={{required: 'Reserve Price is required'}}/>
                    <DateInput name="auctionEnd"
                               control={control}
                               label='Auction end date/time'
                               dateFormat='dd MMMM yyyy h:mm a'
                               showTimeSelect
                               rules={{required: 'Auction End Date is required'}}/>
                </div>
            </>
            }
            <div className='flex justify-between'>
                <Button outline color='gray'>Cancel</Button>
                <Button outline
                        isProcessing={isSubmitting}
                        color='success'
                        type='submit'
                        disabled={!isValid}
                >Submit</Button>
            </div>
        </form>
    )
}
