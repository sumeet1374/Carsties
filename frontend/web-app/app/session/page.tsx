import React from 'react'
import { getSession, getTokenWorkaround } from '../actions/authActions'
import Heading from '../components/Heading';
import AuthTest from './AuthTest';

export  default async function Session() {
  const session = await getSession();
  const token = await getTokenWorkaround();

  return (
    <div>
        <Heading title="Session Dashboard"  /> 
        <div className='bg-blue-200  border-blue-500 '>
            <h3 className='text-lg'>Session Data</h3>
            <pre>{JSON.stringify(session,null,2)}</pre>
        </div>
        <div className='m-4'>
          <AuthTest/>
        </div>
        <div className='bg-blue-200  border-green-500 mt-4'>
            <h3 className='text-lg'>Toekn Data</h3>
            <pre>{JSON.stringify(token,null,2)}</pre>
        </div>
    </div>
  )
}
