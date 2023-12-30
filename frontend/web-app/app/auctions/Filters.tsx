import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import { ButtonGroup,Button } from 'flowbite-react';
import React from 'react'
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill, BsStopwatch} from 'react-icons/bs';
import { GiFinishLine, GiFlame} from 'react-icons/gi';


const pageSizeButtons = [4,8,12];
const orderButtons = [
    { label:'Alphabetical',
      icon:AiOutlineSortAscending,
      value:'make'
    },
    { label:'End Date',
      icon:AiOutlineClockCircle,
      value:'sendingSoon'
    },
    { label:'Recently Added',
      icon:BsFillStopCircleFill,
      value:'new'
    }
];

const filterButtons = [
    { label:'Live Auctions',
      icon:GiFlame,
      value:'live'
    },
    { label:'Ending < 6hrs',
      icon:GiFinishLine,
      value:'endingSoon'
    },
    { label:'Completed',
      icon:BsStopwatch,
      value:'finished'
    }
];


export default function Filters() {



    const setParams = useParamsStore((state)=> state.setParams);
    
    const pageSize = useParamsStore((state)=> state.pageSize);
    const orderBy = useParamsStore((state)=> state.orderBy);
    function setPageSize(pageSize:number) {
        setParams({ pageSize:pageSize})
    }

  return (
    <div className='flex justify-between items-center mb-4'>
         <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Filter By</span>
            <ButtonGroup>
                {filterButtons.map(({label,icon:Icon,value})=> (
                    <Button
                    key={value}
                    onClick={()=>setParams({ filterBy:value})}
                    color={`${orderBy === value?'red':'gray'}`}
                >
                    <Icon className='mr-3 h-4 w-4'/>
                    {label}
                </Button>
                ))}
            </ButtonGroup>
        </div>
        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Order By</span>
            <ButtonGroup>
                {orderButtons.map(({label,icon:Icon,value})=> (
                    <Button
                    key={value}
                    onClick={()=>setParams({ orderBy:value})}
                    color={`${orderBy === value?'red':'gray'}`}
                >
                    <Icon className='mr-3 h-4 w-4'/>
                    {label}
                </Button>
                ))}
            </ButtonGroup>
        </div>
        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Page Size</span>
            <ButtonGroup>
                {pageSizeButtons.map((value,index)=> {
                    return <Button 
                    key={index} 
                    onClick={()=>setPageSize(value)}
                    className='focus:ring-0'
                    color={`${pageSize === value?'red':'gray'}`}
                    >
                        {value}
                    </Button>
                })}
            </ButtonGroup>
        </div>
    </div>
  )
}
